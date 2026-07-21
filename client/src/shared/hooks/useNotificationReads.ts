import { useEffect, useState } from 'react';

const STORAGE_KEY = 'jplearn_read_notifications';
const CHANGE_EVENT = 'jplearn_read_notifications_change';

const getTodayKey = () => new Date().toDateString();

const readStoredIds = () => {
  try {
    const savedStr = localStorage.getItem(STORAGE_KEY);
    if (!savedStr) return [];

    const saved = JSON.parse(savedStr);
    if (saved.date !== getTodayKey()) return [];

    return Array.isArray(saved.readIds) ? saved.readIds : [];
  } catch {
    return [];
  }
};

const saveReadIds = (readIds: number[]) => {
  localStorage.setItem(
    STORAGE_KEY,
    JSON.stringify({ date: getTodayKey(), readIds })
  );
  window.dispatchEvent(new Event(CHANGE_EVENT));
};

export const useNotificationReads = () => {
  const [readIds, setReadIds] = useState<number[]>(readStoredIds);

  useEffect(() => {
    const syncReadIds = () => setReadIds(readStoredIds());

    window.addEventListener('storage', syncReadIds);
    window.addEventListener(CHANGE_EVENT, syncReadIds);

    return () => {
      window.removeEventListener('storage', syncReadIds);
      window.removeEventListener(CHANGE_EVENT, syncReadIds);
    };
  }, []);

  const markAsRead = (notificationId: number) => {
    const currentReadIds = readStoredIds();
    if (currentReadIds.includes(notificationId)) return;

    const nextReadIds = [...currentReadIds, notificationId];
    saveReadIds(nextReadIds);
    setReadIds(nextReadIds);
  };

  const markAllAsRead = (notificationIds: number[]) => {
    const nextReadIds = Array.from(new Set(notificationIds));
    saveReadIds(nextReadIds);
    setReadIds(nextReadIds);
  };

  return {
    readIds,
    markAsRead,
    markAllAsRead,
  };
};
