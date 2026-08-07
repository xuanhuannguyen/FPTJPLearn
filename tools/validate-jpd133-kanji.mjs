import { readFile, readdir } from 'node:fs/promises';
import path from 'node:path';

const root = process.cwd();
const readJson = async (file) => JSON.parse(await readFile(path.join(root, file), 'utf8'));
const core = await readJson('material/KANJI/jpd133_core.json');
const vocab = await readJson('material/KANJI/jpd133_vocab.json');

if (core.length !== 4) throw new Error(`Expected 4 lessons, found ${core.length}`);
const kanjiCount = core.reduce((sum, lesson) => sum + lesson.kanjiItems.length, 0);
if (kanjiCount !== 40) throw new Error(`Expected 40 Kanji, found ${kanjiCount}`);
const chars = new Set(core.flatMap((lesson) => lesson.kanjiItems.map((item) => item.character)));
const totalVocabulary = vocab.reduce((sum, lesson) => sum + lesson.vocabulary.length, 0);
if (vocab.length !== 4) throw new Error(`Expected vocabulary for 4 lessons, found ${vocab.length}`);

for (const lesson of core) {
  if (lesson.kanjiItems.length === 0) throw new Error(`Lesson ${lesson.lessonNumber} has no Kanji`);
  for (const item of lesson.kanjiItems) {
    if (!item.character || !item.meaning || !item.strokeDataJson || !item.componentMapJson) {
      throw new Error(`Incomplete Kanji item: ${lesson.lessonNumber}/${item.character}`);
    }
    const strokeData = JSON.parse(item.strokeDataJson);
    if (strokeData.strokes.length !== item.strokeCount) {
      throw new Error(`Stroke count mismatch: ${item.character}: metadata=${item.strokeCount}, data=${strokeData.strokes.length}`);
    }
    if (strokeData.medians.length !== strokeData.strokes.length) {
      throw new Error(`Median count mismatch: ${item.character}`);
    }
    const components = JSON.parse(item.componentMapJson);
    if (!Array.isArray(components) || components.length === 0) throw new Error(`No component map: ${item.character}`);
    if (!components.some((component) => component.isRadical)) throw new Error(`No radical component: ${item.character}`);
  }
}

for (const lesson of vocab) {
  for (const item of lesson.vocabulary) {
    if (!item.word || !item.reading || !item.meaning) throw new Error(`Incomplete vocabulary: ${lesson.lessonNumber}/${item.word}`);
    if (!item.kanjiCharacters.every((character) => chars.has(character))) throw new Error(`Unknown Kanji mapping: ${item.word}`);
  }
}

const strokeFiles = (await readdir(path.join(root, 'client', 'public', 'data', 'kanji', 'strokes-jp'))).filter((file) => file.endsWith('.json'));
if (strokeFiles.length !== 40) throw new Error(`Expected 40 local stroke files, found ${strokeFiles.length}`);

console.log(`${kanjiCount} kanji validated`);
console.log(`${core.length} lessons validated`);
console.log(`${totalVocabulary} vocabulary items validated`);
console.log('vocabulary mapping validated');
console.log('stroke data validated');
console.log('component/radical data validated');
