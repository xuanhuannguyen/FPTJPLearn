// In-memory cache: path → pending Promise (avoids duplicate fetches)
const cache = new Map<string, Promise<unknown>>();

/**
 * Fetch a static JSON file from /data/, cached for the session.
 * @param path  relative path under /data/, e.g. "vocabulary/courses.json"
 */
export function fetchStatic<T>(path: string): Promise<T> {
  if (!cache.has(path)) {
    const promise = fetch(`/data/${path}`)
      .then((res) => {
        if (!res.ok) {
          throw new Error(`Static data not found: /data/${path} (${res.status})`);
        }
        return res.json() as Promise<T>;
      });
    cache.set(path, promise);
  }
  return cache.get(path) as Promise<T>;
}

/** Clear cache (useful for testing) */
export function clearStaticCache(): void {
  cache.clear();
}
