import { mkdir, readFile, writeFile } from 'node:fs/promises';
import path from 'node:path';

const root = process.cwd();
const resourcePath = path.join(root, 'docs', 'jpd133', 'resource', 'JPD133_Kanji_Bai_8_11_chi_tiet.md');
const corePath = path.join(root, 'material', 'KANJI', 'jpd133_core.json');
const vocabPath = path.join(root, 'material', 'KANJI', 'jpd133_vocab.json');
const mnemonicPath = path.join(root, 'material', 'KANJI', 'jpd133_mnemonics.json');
const serverImportPath = path.join(root, 'server', 'JPLearn.Infrastructure', 'Data', 'Imports', 'kanji', 'jpd133.json');
const strokesDir = path.join(root, 'client', 'public', 'data', 'kanji', 'strokes-jp');

const componentMap = {
  家: [['宀', 'mái nhà', true, 'top'], ['豕', 'con lợn', false, 'bottom']],
  族: [['方', 'phương', false, 'left'], ['矢', 'mũi tên', true, 'right']],
  父: [['父', 'cha', true, 'whole']], 母: [['母', 'mẹ', true, 'whole']],
  兄: [['口', 'miệng', true, 'top'], ['儿', 'người', false, 'bottom']],
  弟: [['弓', 'cung', true, 'left'], ['丿', 'nét phẩy', false, 'right']],
  姉: [['女', 'nữ', true, 'left'], ['市', 'thị', false, 'right']],
  妹: [['女', 'nữ', true, 'left'], ['未', 'chưa', false, 'right']],
  犬: [['犬', 'chó', true, 'whole']], 高: [['高', 'cao', true, 'whole']],
  長: [['長', 'dài', true, 'whole']], 短: [['矢', 'mũi tên', true, 'left'], ['豆', 'đậu', false, 'right']],
  好: [['女', 'nữ', true, 'left'], ['子', 'trẻ em', false, 'right']],
  歌: [['可', 'có thể', false, 'left'], ['欠', 'thiếu', true, 'right']],
  音: [['日', 'mặt trời', true, 'bottom'], ['立', 'đứng', false, 'top']],
  楽: [['白', 'trắng', true, 'top'], ['木', 'cây', false, 'bottom']],
  車: [['車', 'xe', true, 'whole']], 映: [['日', 'mặt trời', true, 'left'], ['央', 'trung tâm', false, 'right']],
  画: [['一', 'một', false, 'top'], ['田', 'ruộng', true, 'center'], ['凵', 'miệng mở', false, 'bottom']],
  旅: [['方', 'phương', true, 'top'], ['𠂉', 'người', false, 'left'], ['氏', 'thị', false, 'bottom']],
  海: [['氵', 'nước', true, 'left'], ['毎', 'mỗi', false, 'right']],
  外: [['夕', 'đêm', true, 'left'], ['卜', 'bói', false, 'right']],
  駅: [['馬', 'ngựa', true, 'left'], ['尺', 'thước', false, 'right']],
  上: [['一', 'một', false, 'top'], ['卜', 'bói', true, 'bottom']],
  下: [['一', 'một', false, 'top'], ['卜', 'bói', true, 'bottom']],
  地: [['土', 'đất', true, 'left'], ['也', 'cũng', false, 'right']],
  図: [['囗', 'vây quanh', true, 'outer'], ['乂', 'cắt', false, 'inner']],
  館: [['飠', 'thực', true, 'left'], ['官', 'quan', false, 'right']],
  右: [['𠂇', 'tay trái', true, 'top'], ['口', 'miệng', false, 'bottom']],
  左: [['𠂇', 'tay trái', true, 'top'], ['工', 'công', false, 'bottom']],
  道: [['辶', 'sước', true, 'left'], ['首', 'thủ', false, 'right']],
  起: [['走', 'tẩu', true, 'left'], ['己', 'kỷ', false, 'right']],
  歩: [['止', 'chỉ', true, 'top'], ['少', 'thiểu', false, 'bottom']],
  始: [['女', 'nữ', true, 'left'], ['台', 'đài', false, 'right']],
  終: [['糸', 'mịch', true, 'left'], ['冬', 'đông', false, 'right']],
  勉: [['免', 'miễn', true, 'top'], ['力', 'lực', false, 'bottom']],
  強: [['弓', 'cung', true, 'left'], ['虫', 'trùng', false, 'right']],
  朝: [['月', 'nguyệt', true, 'left'], ['十', 'thập', false, 'center'], ['日', 'nhật', false, 'right']],
  昼: [['尺', 'xích', false, 'top'], ['日', 'nhật', true, 'bottom']],
  夜: [['亠', 'đầu', false, 'top'], ['夂', 'trĩ', false, 'center'], ['夕', 'tịch', true, 'bottom']],
};

const clean = (value) => value.replaceAll('**', '').trim();
const cells = (line) => line.split('|').slice(1, -1).map((value) => clean(value));
const kanjiPattern = /[一-龥々]/u;
const strokeFallbacks = { 姉: '姊', 駅: '驿', 図: '圖', 楽: '樂', 歩: '步' };

const parseCore = (markdown) => {
  const items = [];
  let lessonNumber = null;
  let inCoreTable = false;
  for (const line of markdown.split(/\r?\n/u)) {
    const section = line.match(/^## (8|9|10|11)(?:_|-)1/u);
    if (section) {
      lessonNumber = Number(section[1]);
      inCoreTable = false;
      continue;
    }
    if (line.startsWith('## ') && !line.match(new RegExp(`^## ${lessonNumber}(?:_|-)1`, 'u'))) {
      inCoreTable = false;
    }
    if (line.startsWith('| STT | Mã | Kanji |')) {
      inCoreTable = true;
      continue;
    }
    if (!inCoreTable || !line.match(/^\|\s*\d+\s*\|/u)) continue;
    const [order, code, character, hanViet, meaning, onReading, kunReading, strokeCount, compounds] = cells(line);
    if (!character || !Number.isInteger(Number(strokeCount))) continue;
    items.push({
      level: 'N5', lessonNumber, lessonCode: code, character, hanViet, meaning,
      onReading: onReading === '—' ? '' : onReading,
      kunReading: kunReading === '—' ? '' : kunReading,
      strokeCount: Number(strokeCount),
      mnemonic: `Ghi nhớ ${character} qua các thành phần: ${(componentMap[character] || []).map((item) => item[0]).join(' + ') || character}.`,
      componentMapJson: JSON.stringify((componentMap[character] || [[character, meaning, true, 'whole']]).map(([component, name, isRadical, position]) => ({
        character: component, component, name, meaning: name, isRadical, position,
      }))),
      compounds,
      orderIndex: Number(order),
    });
  }
  if (items.length !== 40) throw new Error(`Expected 40 Kanji, found ${items.length}`);
  return items;
};

const parseVocabulary = (core, markdown) => {
  const byCharacter = new Map(core.map((item) => [item.character, item]));
  const byLesson = new Map();
  const add = (lessonNumber, word, reading, meaning, sourceCharacter) => {
    if (!word || !meaning) return;
    const chars = [...word].filter((char) => byCharacter.has(char));
    if (sourceCharacter && chars.length === 0) chars.push(sourceCharacter);
    const lesson = byLesson.get(lessonNumber) ?? new Map();
    const key = `${word}|${reading}|${meaning}`;
    const existing = lesson.get(key);
    if (existing) {
      existing.kanjiCharacters = [...new Set([...existing.kanjiCharacters, ...chars])];
    } else {
      lesson.set(key, { word, reading: reading || word, meaning, kanjiCharacters: chars });
    }
    byLesson.set(lessonNumber, lesson);
  };

  let lessonNumber = null;
  let inCoreTable = false;
  for (const line of markdown.split(/\r?\n/u)) {
    const section = line.match(/^## (8|9|10|11)(?:_|-)1/u);
    if (section) { lessonNumber = Number(section[1]); inCoreTable = false; continue; }
    if (line.startsWith('## ') && !line.match(new RegExp(`^## ${lessonNumber}(?:_|-)1`, 'u'))) inCoreTable = false;
    if (line.startsWith('| STT | Mã | Kanji |')) { inCoreTable = true; continue; }
    if (!inCoreTable || !line.match(/^\|\s*\d+\s*\|/u)) continue;
    const row = cells(line);
    const character = row[2];
    const compounds = row[8] || '';
    for (const segment of compounds.split(';')) {
      const normalized = segment.trim();
      if (!normalized) continue;
      const withReading = normalized.match(/^(.+?)（([^）]+)）\s*[–-]\s*(.+)$/u);
      const withoutReading = normalized.match(/^(.+?)\s*[–-]\s*(.+)$/u);
      const match = withReading || withoutReading;
      if (!match) continue;
      const word = match[1].trim();
      const reading = withReading ? match[2].trim() : word;
      const meaning = (withReading ? match[3] : match[2]).trim();
      add(lessonNumber, word, reading, meaning, character);
    }
  }

  return [8, 9, 10, 11].map((lessonNumber) => ({
    lessonNumber,
    vocabulary: [...(byLesson.get(lessonNumber) || new Map()).values()].map((item, index) => ({
      ...item,
      exampleJapanese: `${item.word}です。`,
      exampleReading: `${item.reading}です。`,
      exampleMeaning: `${item.meaning}。`,
      orderIndex: index + 1,
    })),
  }));
};

const fetchJson = async (url) => {
  const response = await fetch(url);
  if (!response.ok) throw new Error(`Failed to fetch ${url}: ${response.status}`);
  return response.json();
};

const fetchStrokeData = async (character) => {
  const encoded = encodeURIComponent(character);
  let sourceCharacter = character;
  let data;
  try {
    data = await fetchJson(`https://cdn.jsdelivr.net/npm/hanzi-writer-data-jp@0.0.1/${encoded}.json`);
  } catch (error) {
    sourceCharacter = strokeFallbacks[character];
    if (!sourceCharacter) throw error;
    data = await fetchJson(`https://cdn.jsdelivr.net/npm/hanzi-writer-data@2.0/${encodeURIComponent(sourceCharacter)}.json`);
  }
  const result = { character, strokes: data.strokes || [], medians: data.medians || [], strokeCount: data.strokes?.length || 0, source: sourceCharacter === character ? 'hanzi-writer-data-jp@0.0.1' : `hanzi-writer-data@2.0 fallback:${sourceCharacter}` };
  await writeFile(path.join(strokesDir, `${character}.json`), `${JSON.stringify(result, null, 2)}\n`, 'utf8');
  return result;
};

const main = async () => {
  await mkdir(strokesDir, { recursive: true });
  const markdown = await readFile(resourcePath, 'utf8');
  const mnemonics = JSON.parse(await readFile(mnemonicPath, 'utf8'));
  const core = parseCore(markdown);
  for (const item of core) item.mnemonic = mnemonics[item.character] || item.mnemonic;
  const vocabulary = parseVocabulary(core, markdown);
  const strokeData = new Map();
  for (const item of core) strokeData.set(item.character, await fetchStrokeData(item.character));
  const coreWithStrokes = core.map((item) => ({
    level: item.level,
    lessonNumber: item.lessonNumber,
    lessonTitle: ({ 8: 'Gia đình và đời sống', 9: 'Sở thích, âm nhạc, du lịch, nước ngoài', 10: 'Địa điểm, phương hướng, đường đi', 11: 'Sinh hoạt hằng ngày, học tập, thời gian' })[item.lessonNumber],
    accessTier: item.lessonNumber === 8 ? 'free' : 'premium',
    kanjiItems: [],
  })).filter((item, index, all) => all.findIndex((candidate) => candidate.lessonNumber === item.lessonNumber) === index)
    .map((lesson) => ({
      ...lesson,
      kanjiItems: core.filter((item) => item.lessonNumber === lesson.lessonNumber).map((item) => {
        const strokes = strokeData.get(item.character);
        return {
          character: item.character,
          hanViet: item.hanViet,
          meaning: item.meaning,
          strokeCount: strokes.strokeCount || item.strokeCount,
          kunReading: item.kunReading,
          onReading: item.onReading,
          mnemonic: item.mnemonic,
          strokeSvg: null,
          strokeDataJson: JSON.stringify({ strokes: strokes.strokes, medians: strokes.medians }),
          componentMapJson: item.componentMapJson,
          orderIndex: item.orderIndex,
          sourceCode: item.lessonCode,
        };
      }),
    }));
  await writeFile(corePath, `${JSON.stringify(coreWithStrokes, null, 2)}\n`, 'utf8');
  await writeFile(vocabPath, `${JSON.stringify(vocabulary, null, 2)}\n`, 'utf8');
  await mkdir(path.dirname(serverImportPath), { recursive: true });
  await writeFile(serverImportPath, `${JSON.stringify({ courseCode: 'jpd133', lessons: coreWithStrokes.map((lesson) => ({ ...lesson, vocabulary: vocabulary.find((item) => item.lessonNumber === lesson.lessonNumber)?.vocabulary || [] })) }, null, 2)}\n`, 'utf8');
  console.log(`Generated ${core.length} Kanji, ${vocabulary.reduce((sum, lesson) => sum + lesson.vocabulary.length, 0)} vocabulary items and ${strokeData.size} local stroke files.`);
};

await main();
