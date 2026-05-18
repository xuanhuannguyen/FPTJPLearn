type AccessControlledContent = {
  accessTier?: string | null;
  packageCode?: string | null;
  isLocked?: boolean | null;
};

export function clearUserAccessCache() {
  // Access is currently free for every user, so there is no subscription cache.
}

export function useUserAccess() {
  const hasCourseAccess = (packageCode?: string | null) => {
    void packageCode;
    return true;
  };
  const isPackageLocked = (packageCode?: string | null) => {
    void packageCode;
    return false;
  };
  const isContentLocked = (content?: AccessControlledContent | null) => {
    void content;
    return false;
  };

  return {
    activeCourseCodes: [],
    isLoading: false,
    hasCourseAccess,
    isPackageLocked,
    isContentLocked,
  };
}
