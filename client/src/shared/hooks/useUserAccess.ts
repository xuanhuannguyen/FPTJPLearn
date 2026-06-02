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

const ACCESS_CACHE_TTL_MS = 30_000;

let cachedAccessStatus: AccessStatus | null = null;
let cachedAccessStatusAt = 0;
let pendingAccessRequest: Promise<AccessStatus> | null = null;

export function clearUserAccessCache() {
  cachedAccessStatus = null;
  cachedAccessStatusAt = 0;
  pendingAccessRequest = null;
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
  const [accessStatus, setAccessStatus] = useState<AccessStatus>(cachedAccessStatus ?? defaultAccessStatus);
  const [isLoading, setIsLoading] = useState(!cachedAccessStatus);

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

    const intervalId = window.setInterval(() => {
      updateAccessStatus(true);
    }, ACCESS_CACHE_TTL_MS);

    const handleFocus = () => updateAccessStatus(true);
    window.addEventListener('focus', handleFocus);

    return () => {
      isMounted = false;
      window.clearInterval(intervalId);
      window.removeEventListener('focus', handleFocus);
    };
  }, []);

  const hasCourseAccess = useCallback((packageCode?: string | null) => {
    if (!accessStatus.licensingEnabled) return true;

    const courseCode = mapPackageToCourseCode(packageCode);
    if (!courseCode) return false;

    return accessStatus.activeCourseCodes.includes(courseCode);
  }, [accessStatus]);

  const isPackageLocked = useCallback((packageCode?: string | null) => {
    return !hasCourseAccess(packageCode);
  }, [hasCourseAccess]);

  const isContentLocked = useCallback((content?: AccessControlledContent | null) => {
    if (!content || !accessStatus.licensingEnabled) return false;

    const accessTier = content.accessTier?.trim().toLowerCase() || 'free';
    if (accessTier === 'free') return false;

    if (content.packageCode) {
      return isPackageLocked(content.packageCode);
    }

    return typeof content.isLocked === 'boolean' ? content.isLocked : true;
  }, [accessStatus, isPackageLocked]);

  return {
    activeCourseCodes: accessStatus.activeCourseCodes,
    subscriptions: accessStatus.subscriptions,
    licensingEnabled: accessStatus.licensingEnabled,
    freeExperienceEnabled: accessStatus.freeExperienceEnabled,
    isLoading,
    hasCourseAccess,
    isPackageLocked,
    isContentLocked,
  };
}
