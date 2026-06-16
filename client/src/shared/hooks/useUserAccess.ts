import { useCallback, useEffect, useState } from 'react';
import { apiClient } from '../api/axios';

type AccessControlledContent = {
  accessTier?: string | null;
  packageCode?: string | null;
  isLocked?: boolean | null;
};

type AccessSubscription = {
  courseCode: string;
  expiresAt: string;
  isActive: boolean;
};

type AccessStatus = {
  licensingEnabled: boolean;
  freeExperienceEnabled: boolean;
  activeCourseCodes: string[];
  subscriptions: AccessSubscription[];
};

const defaultAccessStatus: AccessStatus = {
  licensingEnabled: true,
  freeExperienceEnabled: false,
  activeCourseCodes: [],
  subscriptions: [],
};

const ACCESS_CACHE_TTL_MS = 10 * 60 * 1000;
const ACCESS_CACHE_STORAGE_KEY = 'jplearn_access_status_cache';

let cachedAccessStatus: AccessStatus | null = null;
let cachedAccessStatusAt = 0;
let pendingAccessRequest: Promise<AccessStatus> | null = null;

function readStoredAccessStatus() {
  try {
    const raw = sessionStorage.getItem(ACCESS_CACHE_STORAGE_KEY);
    if (!raw) return null;

    const parsed = JSON.parse(raw) as { status?: AccessStatus; cachedAt?: number };
    if (!parsed.status || !parsed.cachedAt) return null;
    if (Date.now() - parsed.cachedAt >= ACCESS_CACHE_TTL_MS) return null;

    return parsed;
  } catch {
    return null;
  }
}

function writeStoredAccessStatus(status: AccessStatus, cachedAt: number) {
  try {
    sessionStorage.setItem(ACCESS_CACHE_STORAGE_KEY, JSON.stringify({ status, cachedAt }));
  } catch {
    // Ignore storage failures; in-memory cache still works for the current page.
  }
}

function removeStoredAccessStatus() {
  try {
    sessionStorage.removeItem(ACCESS_CACHE_STORAGE_KEY);
  } catch {
    // Ignore storage failures.
  }
}

function getInitialAccessStatus() {
  const stored = readStoredAccessStatus();
  if (!stored) return null;

  cachedAccessStatus = stored.status;
  cachedAccessStatusAt = stored.cachedAt;
  return stored.status;
}

const initialAccessStatus = getInitialAccessStatus();

export function clearUserAccessCache() {
  cachedAccessStatus = null;
  cachedAccessStatusAt = 0;
  pendingAccessRequest = null;
  removeStoredAccessStatus();
}

export function setUserAccessCache(status: AccessStatus) {
  cachedAccessStatus = status;
  cachedAccessStatusAt = Date.now();
  pendingAccessRequest = null;
  writeStoredAccessStatus(status, cachedAccessStatusAt);
}

export async function refreshUserAccessCache() {
  return fetchAccessStatus(true);
}

async function fetchAccessStatus(forceRefresh = false) {
  const isCacheFresh = cachedAccessStatus && Date.now() - cachedAccessStatusAt < ACCESS_CACHE_TTL_MS;
  if (!forceRefresh && isCacheFresh) return cachedAccessStatus;

  if (!pendingAccessRequest) {
    pendingAccessRequest = apiClient
      .get<AccessStatus>('/access/me')
      .then((response) => {
        cachedAccessStatus = response.data;
        cachedAccessStatusAt = Date.now();
        writeStoredAccessStatus(response.data, cachedAccessStatusAt);
        return response.data;
      })
      .catch((error) => {
        console.error('Failed to fetch access status:', error);
        return defaultAccessStatus;
      })
      .finally(() => {
        pendingAccessRequest = null;
      });
  }

  return pendingAccessRequest;
}

function mapPackageToCourseCode(packageCode?: string | null) {
  if (!packageCode) return null;
  const code = packageCode.trim().toLowerCase();

  if (code.includes('jpd113')) return 'jpd113';
  if (code.includes('jpd123')) return 'jpd123';

  return code;
}

export function useUserAccess() {
  const [accessStatus, setAccessStatus] = useState<AccessStatus>(cachedAccessStatus ?? initialAccessStatus ?? defaultAccessStatus);
  const [isLoading, setIsLoading] = useState(!cachedAccessStatus && !initialAccessStatus);

  useEffect(() => {
    let isMounted = true;

    const updateAccessStatus = (forceRefresh = false) => {
      fetchAccessStatus(forceRefresh).then((status) => {
        if (!isMounted) return;
        setAccessStatus(status);
        setIsLoading(false);
      });
    };

    updateAccessStatus();

    const handleFocus = () => updateAccessStatus();
    window.addEventListener('focus', handleFocus);

    return () => {
      isMounted = false;
      window.removeEventListener('focus', handleFocus);
    };
  }, []);

  const hasCourseAccess = useCallback((packageCode?: string | null) => {
    if (isLoading) return true;
    if (!accessStatus.licensingEnabled) return true;

    const courseCode = mapPackageToCourseCode(packageCode);
    if (!courseCode) return false;

    return accessStatus.activeCourseCodes.includes(courseCode);
  }, [accessStatus, isLoading]);

  const isPackageLocked = useCallback((packageCode?: string | null) => {
    return !hasCourseAccess(packageCode);
  }, [hasCourseAccess]);

  const isContentLocked = useCallback((content?: AccessControlledContent | null) => {
    if (isLoading) return false;
    if (!content || !accessStatus.licensingEnabled) return false;

    const accessTier = content.accessTier?.trim().toLowerCase() || 'free';
    if (accessTier === 'free') return false;

    if (content.packageCode) {
      return isPackageLocked(content.packageCode);
    }

    return typeof content.isLocked === 'boolean' ? content.isLocked : true;
  }, [accessStatus, isLoading, isPackageLocked]);

  return {
    activeCourseCodes: accessStatus.activeCourseCodes,
    subscriptions: accessStatus.subscriptions,
    licensingEnabled: isLoading ? false : accessStatus.licensingEnabled,
    freeExperienceEnabled: isLoading ? true : accessStatus.freeExperienceEnabled,
    isLoading,
    hasCourseAccess,
    isPackageLocked,
    isContentLocked,
  };
}
