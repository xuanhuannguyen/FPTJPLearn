import { readFile, writeFile } from 'node:fs/promises';

const sourcePath = 'docs/JPD133/Resource/JPD133_Tu_vung_Bai_8_11_chi_tiet.md';
const targetPath = 'server/JPLearn.Infrastructure/Data/Imports/vocabulary/jpd133.lessons.json';
const markdown = await readFile(sourcePath, 'utf8');

const readingFixes = new Map([
  ['～人', 'にん'], ['～匹', 'ひき'], ['～日', 'にち'], ['～週間', 'しゅうかん'], ['～か月', 'かげつ'],
  ['一か月', 'いっかげつ'], ['六か月', 'ろっかげつ'], ['八か月', 'はっかげつ'], ['十か月', 'じゅっかげつ'], ['何か月', 'なんかげつ'],
  ['～年', 'ねん'], ['～回', 'かい'], ['一回', 'いっかい'], ['六回', 'ろっかい'], ['八回', 'はっかい'], ['十回', 'じゅっかい'], ['何回', 'なんかい'],
  ['～冊', 'さつ'], ['一冊', 'いっさつ'], ['十冊', 'じゅっさつ'], ['何冊', 'なんさつ'], ['～杯', 'はい'], ['一杯', 'いっぱい'],
  ['六杯', 'ろっぱい'], ['八杯', 'はっぱい'], ['十杯', 'じゅっぱい'], ['何杯', 'なんばい'], ['～本', 'ほん'], ['一本', 'いっぽん'],
  ['六本', 'ろっぽん'], ['八本', 'はっぽん'], ['十本', 'じゅっぽん'], ['何本', 'なんぼん'], ['～料理（例：イタリア料理）～りょうり', 'りょうり'],
  ['～クラブ（例：ダンスクラブ）', 'クラブ'], ['～教室（例：）きょうしつ', 'きょうしつ'], ['～番', 'ばん'], ['～つ目', 'つめ'], ['～たち', 'たち'],
  ['なります「なる」', 'なります'], ['あそこのベンチで休みましょう', 'あそこのベンチでやすみましょう'],
]);

const clean = (value) => value.replaceAll('`', '').replaceAll('**', '').replace(/\s+/gu, ' ').trim();
const sectionRegex = /^## (8_[1-3]|9_[1-3]|10_[1-3]|11_[1-3]) – ([^/]+) \/ ([^\n]+)$/gmu;
const sections = [...markdown.matchAll(sectionRegex)];
const report = { sectionCount: sections.length, lessonCounts: {}, missingReading: [], duplicateItems: [], missingExamples: [], placeholders: [], removedItems: [] };
if (sections.length !== 12) throw new Error(`Expected 12 vocabulary sections, found ${sections.length}`);

const sectionItems = new Map();
for (let index = 0; index < sections.length; index += 1) {
  const [, sectionCode, japaneseTitle, vietnameseTitle] = sections[index];
  const start = sections[index].index + sections[index][0].length;
  const end = sections[index + 1]?.index ?? markdown.length;
  const block = markdown.slice(start, end);
  const rows = [...block.matchAll(/^\| (\d+) \| ([^\n]+)$/gmu)];
  const items = [];
  for (const row of rows) {
    const cells = row[2].split('|').map(clean);
    if (cells.length < 9) {
      report.removedItems.push(`${sectionCode}#${row[1]} malformed row`);
      continue;
    }
    const [word, rawReading, wordType, meaning, exampleJapanese, exampleReading, exampleMeaning, notes] = cells;
    const reading = rawReading === '—' ? readingFixes.get(word) ?? '' : rawReading;
    if (!reading) report.missingReading.push(`${sectionCode}: ${word}`);
    if (!word || !meaning) report.removedItems.push(`${sectionCode}#${row[1]} missing word/meaning`);
    if (!exampleJapanese || !exampleReading || !exampleMeaning) report.missingExamples.push(`${sectionCode}: ${word}`);
    if (/\[CẦN BỔ SUNG\]|\[điền\]|\[ \]/u.test(row[2])) report.placeholders.push(`${sectionCode}: ${word}`);
    items.push({ word, reading, wordType, meaning, exampleJapanese, exampleReading, exampleMeaning, notes: `${notes} Nguồn: ${sectionCode}` });
  }
  sectionItems.set(sectionCode, { japaneseTitle: clean(japaneseTitle), vietnameseTitle: clean(vietnameseTitle), items });
}

const lessons = [8, 9, 10, 11].map((lessonNumber) => {
  const lessonSections = [1, 2, 3].map((part) => `${lessonNumber}_${part}`);
  const seen = new Map();
  const items = [];
  for (const sectionCode of lessonSections) {
    for (const item of sectionItems.get(sectionCode).items) {
      const key = `${item.word}\u0000${item.reading}\u0000${item.meaning}`;
      if (seen.has(key)) {
        report.duplicateItems.push(`${sectionCode}: ${item.word}`);
        seen.get(key).notes += `; ${item.notes}`;
        continue;
      }
      seen.set(key, item);
      items.push({ ...item, orderIndex: items.length + 1 });
    }
  }
  report.lessonCounts[lessonNumber] = items.length;
  return {
    id: lessonNumber,
    title: `JPD133 Bài ${lessonNumber}`,
    description: lessonNumber === 8 ? 'Gia đình, bạn bè và quà tặng.' : lessonNumber === 9 ? 'Sở thích, khả năng và cuối tuần.' : lessonNumber === 10 ? 'Chỉ đường, lưu ý và động vật trong vườn thú.' : 'Cuộc sống hiện tại, quá khứ và hội thoại bạn bè.',
    accessTier: lessonNumber === 8 ? 'free' : 'premium',
    packageCode: 'jpd133',
    orderIndex: lessonNumber,
    items,
  };
});

const output = { courseCode: 'jpd133', title: 'Tiếng Nhật Sơ Cấp 3', description: 'Từ vựng JPD133 Bài 8–11', lessons };
console.log(JSON.stringify(report, null, 2));
if (report.missingReading.length || report.missingExamples.length || report.placeholders.length || report.removedItems.length) {
  throw new Error('Vocabulary import validation failed; see report above.');
}
await writeFile(targetPath, `${JSON.stringify(output, null, 2)}\n`, 'utf8');
console.log(`Imported ${lessons.length} lessons and ${lessons.reduce((sum, lesson) => sum + lesson.items.length, 0)} items.`);
