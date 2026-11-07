import { mkdir, readFile, writeFile } from 'node:fs/promises';
import path from 'node:path';

const root = process.cwd();
const sourcePath = path.join(root, 'material/Luyện Nói/JPD133_25_Readings.tsv');
const importPath = path.join(root, 'server/JPLearn.Infrastructure/Data/Imports/speaking/jpd133.lessons.json');
const markdownPath = path.join(root, 'docs/JPD133/Resource/JPD133_LuyệnDọcĐoạnVawn/JPD133_Reading_Mixed.md');

const sourceText = await readFile(sourcePath, 'utf8');
const blocks = sourceText.trim().split(/\n(?=## )/u);
const lessons = blocks.map((block, index) => {
  const [header, ...sentenceLines] = block.split(/\r?\n/u);
  const match = header.match(/^## (\d+) \| ([^|]+) \| ([^|]+) \| ([^|]+) \| (.+)$/u);
  if (!match) throw new Error(`Invalid story header at index ${index + 1}`);
  const [, idText, title, topic, focus, summary] = match;
  const sentences = sentenceLines.filter(Boolean).map((line, sentenceIndex) => {
    const fields = line.split('\t').map((field) => field.trim());
    if (fields.length !== 3 || fields.some((field) => !field)) {
      throw new Error(`Expected Japanese, Vietnamese, romaji in lesson ${idText}, sentence ${sentenceIndex + 1}`);
    }
    const [jp, vi, romaji] = fields;
    if (!/[。！？]$/u.test(jp) || vi.length < 8 || !/^[a-z]/iu.test(romaji)) {
      throw new Error(`Incomplete sentence in lesson ${idText}, sentence ${sentenceIndex + 1}`);
    }
    return { jp, vi, romaji };
  });
  return {
    id: Number(idText),
    orderIndex: index + 1,
    topic,
    title: title.trim(),
    subtitle: focus.trim(),
    summary: summary.trim(),
    lessonType: 'reading',
    sentences,
  };
});

if (lessons.length !== 25 || lessons.some((lesson, index) => lesson.id !== 801 + index || lesson.sentences.length !== 10)) {
  throw new Error('Expected JPD133 reading lessons 801–825, with 10 complete sentences each.');
}

const importFile = {
  courseCode: 'jpd133',
  accessTier: 'premium',
  packageCode: 'speaking_jpd133',
  lessons,
};

await mkdir(path.dirname(importPath), { recursive: true });
await writeFile(importPath, `${JSON.stringify(importFile, null, 2)}\n`, 'utf8');
await mkdir(path.dirname(markdownPath), { recursive: true });

const markdown = lessons.map((lesson) => [
  `## ${lesson.title}`,
  '',
  `> ${lesson.topic} · ${lesson.subtitle}`,
  `> ${lesson.summary}`,
  '',
  '### Tiếng Nhật',
  lesson.sentences.map((sentence) => sentence.jp).join('\n'),
  '',
  '### Romaji',
  lesson.sentences.map((sentence) => sentence.romaji).join('\n'),
  '',
  '### Tiếng Việt',
  lesson.sentences.map((sentence) => `- ${sentence.vi}`).join('\n'),
  '',
].join('\n')).join('\n---\n\n');
await writeFile(markdownPath, `# JPD133 Reading – 25 mạch chuyện Bài 8–11\n\n${markdown}\n`, 'utf8');

console.log(`Generated ${lessons.length} coherent JPD133 reading lessons with ${lessons.reduce((count, lesson) => count + lesson.sentences.length, 0)} sentences.`);
