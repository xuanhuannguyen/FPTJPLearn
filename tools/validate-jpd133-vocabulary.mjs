import { readFile } from 'node:fs/promises';

const readJson = async (file) => JSON.parse(await readFile(file, 'utf8'));
const importData = await readJson('server/JPLearn.Infrastructure/Data/Imports/vocabulary/jpd133.lessons.json');
const lessons = await readJson('client/public/data/vocabulary/jpd133/lessons.json');
const details = await Promise.all(lessons.map((lesson) => readJson(`client/public/data/vocabulary/jpd133/lessons/${lesson.id}.json`)));
const expectedCodes = ['8_1', '8_2', '8_3', '9_1', '9_2', '9_3', '10_1', '10_2', '10_3', '11_1', '11_2', '11_3'];
const expectedCount = importData.lessons.reduce((sum, lesson) => sum + lesson.items.length, 0);
const allItems = details.flatMap((detail) => detail.items);

if (lessons.length !== 12) throw new Error(`Expected 12 static lessons, found ${lessons.length}`);
if (lessons.some((lesson, index) => lesson.title !== `JPD133 Bài ${expectedCodes[index].replace('_', '.')}`)) throw new Error('Static lessons are not in 8.1 to 11.3 order');
if (allItems.length !== expectedCount) throw new Error(`Item count mismatch: static=${allItems.length}, import=${expectedCount}`);
if (new Set(allItems.map((item) => item.id)).size !== allItems.length) throw new Error('Duplicate static vocabulary item ID');
if (new Set(importData.lessons.flatMap((lesson) => lesson.items).map((item) => item.id)).size !== expectedCount) throw new Error('Duplicate import vocabulary item ID');

const staticById = new Map(allItems.map((item) => [item.id, item]));
for (const item of importData.lessons.flatMap((lesson) => lesson.items)) {
  const staticItem = staticById.get(item.id);
  if (!staticItem || staticItem.word !== item.word || staticItem.reading !== item.reading) throw new Error(`Static/import ID mismatch: ${item.id}`);
}

console.log(`Valid: ${lessons.length} lessons, ${allItems.length} unique vocabulary items, stable IDs.`);
