export const parseGrammarTags = (tagsJson?: string) => {
  if (!tagsJson) {
    return [];
  }

  try {
    const tags = JSON.parse(tagsJson);
    return Array.isArray(tags) ? tags.filter((tag): tag is string => typeof tag === 'string') : [];
  } catch {
    return [];
  }
};
