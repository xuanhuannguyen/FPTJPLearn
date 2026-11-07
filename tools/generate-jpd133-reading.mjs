import { mkdir, readFile, writeFile } from 'node:fs/promises';
import path from 'node:path';

const root = process.cwd();
const sourcePath = path.join(root, 'material/Luyện Nói/JPD133_25_Readings.tsv');
const importPath = path.join(root, 'server/JPLearn.Infrastructure/Data/Imports/speaking/jpd133.lessons.json');
const markdownPath = path.join(root, 'docs/JPD133/Resource/JPD133_LuyệnDọcĐoạnVawn/JPD133_Reading_Mixed.md');

const romajiKana = new Map();
const basicRows = [
  ['', 'あいうえお'], ['k', 'かきくけこ'], ['g', 'がぎぐげご'], ['s', 'さしすせそ'],
  ['z', 'ざじずぜぞ'], ['t', 'たちつてと'], ['d', 'だぢづでど'], ['n', 'なにぬねの'],
  ['h', 'はひふへほ'], ['b', 'ばびぶべぼ'], ['p', 'ぱぴぷぺぽ'], ['m', 'まみむめも'],
  ['r', 'らりるれろ'],
];
for (const [consonant, row] of basicRows) {
  [...row].forEach((kana, index) => romajiKana.set(`${consonant}${'aiueo'[index]}`, kana));
}
for (const [consonant, vowels, row] of [
  ['y', 'auo', 'やゆよ'], ['w', 'ao', 'わを'],
]) {
  [...row].forEach((kana, index) => romajiKana.set(`${consonant}${vowels[index]}`, kana));
}
for (const [romaji, kana] of Object.entries({
  shi: 'し', chi: 'ち', tsu: 'つ', fu: 'ふ', ji: 'じ',
  sha: 'しゃ', shu: 'しゅ', sho: 'しょ', she: 'しぇ',
  cha: 'ちゃ', chu: 'ちゅ', cho: 'ちょ', che: 'ちぇ',
  sya: 'しゃ', syu: 'しゅ', syo: 'しょ', tya: 'ちゃ', tyu: 'ちゅ', tyo: 'ちょ',
  cya: 'ちゃ', cyu: 'ちゅ', cyo: 'ちょ', jya: 'じゃ', jyu: 'じゅ', jyo: 'じょ',
  wi: 'うぃ', we: 'うぇ', wo: 'うぉ', va: 'ゔぁ', vi: 'ゔぃ', vu: 'ゔ', ve: 'ゔぇ', vo: 'ゔぉ',
})) romajiKana.set(romaji, kana);
for (const [cluster, base] of [
  ['ky', 'き'], ['gy', 'ぎ'], ['sh', 'し'], ['j', 'じ'], ['ch', 'ち'], ['ny', 'に'],
  ['hy', 'ひ'], ['by', 'び'], ['py', 'ぴ'], ['my', 'み'], ['ry', 'り'],
]) {
  for (const [vowel, smallKana] of [['a', 'ゃ'], ['u', 'ゅ'], ['o', 'ょ']]) {
    romajiKana.set(`${cluster}${vowel}`, `${base}${smallKana}`);
  }
  if (cluster !== 'sh' && cluster !== 'j' && cluster !== 'ch') {
    romajiKana.set(`${cluster}e`, `${base}ぇ`);
  }
}
romajiKana.set('ye', 'いぇ');
const romajiKeys = [...romajiKana.keys()].sort((a, b) => b.length - a.length);

const kanaVowels = new Map();
for (const [consonant, row] of basicRows) {
  [...row].forEach((kana, index) => kanaVowels.set(kana, 'あいうえお'[index]));
}
for (const [vowels, row] of [['あうお', 'やゆよ'], ['あお', 'わを']]) {
  [...row].forEach((kana, index) => kanaVowels.set(kana, vowels[index]));
}
for (const kana of ['ゃ', 'ぁ']) kanaVowels.set(kana, 'あ');
for (const kana of ['ゅ', 'ぅ']) kanaVowels.set(kana, 'う');
for (const kana of ['ょ', 'ぉ']) kanaVowels.set(kana, 'お');
for (const kana of ['ぃ']) kanaVowels.set(kana, 'い');
for (const kana of ['ぇ']) kanaVowels.set(kana, 'え');

const toHiragana = (text) => {
  let result = '';
  for (const char of text) {
    const hira = char >= 'ァ' && char <= 'ヶ'
      ? String.fromCodePoint(char.codePointAt(0) - 0x60)
      : char;
    if (hira === 'ー') {
      result += kanaVowels.get([...result].at(-1)) ?? hira;
    } else {
      result += hira;
    }
  }
  return result;
};

const transliterateRomaji = (token, source) => {
  let result = '';
  for (let index = 0; index < token.length;) {
    const current = token[index];
    const next = token[index + 1];
    if (current === "'") {
      index += 1;
      continue;
    }
    if (current === 'n') {
      if (next === "'") {
        result += 'ん';
        index += 2;
        continue;
      }
      if (next === 'n') {
        result += 'ん';
        index += 1;
        continue;
      }
      const nextIsVowel = next && 'aiueo'.includes(next);
      const nyStartsMora = next === 'y' && (!index || !'aiueo'.includes(token[index - 1]));
      if (!next || (!nextIsVowel && !nyStartsMora)) {
        result += 'ん';
        index += 1;
        continue;
      }
    }
    if (next === current && current !== 'n' && !'aeiou'.includes(current)) {
      result += 'っ';
      index += 1;
      continue;
    }
    const match = romajiKeys.find((key) => token.startsWith(key, index));
    if (match) {
      result += romajiKana.get(match);
      index += match.length;
      continue;
    }
    if ('aiueo'.includes(current)) {
      result += { a: 'あ', i: 'い', u: 'う', e: 'え', o: 'お' }[current];
      index += 1;
      continue;
    }
    throw new Error(`Cannot read romaji syllable "${token.slice(index)}" in "${source}"`);
  }
  return result;
};

const romanPunctuation = { ',': ',', '.': '.', '!': '!', '?': '?' };
const japanesePunctuation = { '、': ',', '。': '.', '！': '!', '？': '?' };
const romajiToHiragana = (text) => {
  let reading = '';
  const boundaries = [];
  for (const match of text.toLowerCase().matchAll(/[a-z']+|[,.!?]/giu)) {
    const [part] = match;
    if (romanPunctuation[part]) {
      boundaries.push({ offset: reading.length, mark: romanPunctuation[part] });
    } else {
      reading += transliterateRomaji(part, text);
    }
  }
  return { reading, boundaries };
};

const isKanjiOrDigit = (char) => /[\p{Script=Han}0-9０-９]/u.test(char);
const isKana = (char) => /[\p{Script=Hiragana}\p{Script=Katakana}ー]/u.test(char);

const getReadingTokens = (japanese) => {
  const tokens = [];
  let offset = 0;
  let punctuationIndex = 0;
  let canMergePrevious = false;
  for (const char of japanese) {
    const end = offset + char.length;
    const type = isKanjiOrDigit(char) ? 'kanji' : isKana(char) ? 'kana' : null;
    if (type === null && japanesePunctuation[char]) {
      tokens.push({ type: 'punctuation', mark: japanesePunctuation[char], punctuationIndex, start: offset, end });
      punctuationIndex += 1;
      canMergePrevious = false;
    } else if (type) {
      const previous = tokens.at(-1);
      if (canMergePrevious && previous?.type === type) {
        previous.text += char;
        previous.end = end;
      } else {
        tokens.push({ type, text: char, start: offset, end });
      }
      canMergePrevious = true;
    } else {
      // Quotes are not spoken, but they must split adjacent ruby groups.
      canMergePrevious = false;
    }
    offset = end;
  }
  return tokens;
};

const expectedKana = (text, followsKanji) => {
  let target = toHiragana(text);
  if (followsKanji) {
    target = target
      .replace(/([にでと])は/gu, '$1わ')
      .replace(/^は/u, 'わ')
      .replace(/^へ/u, 'え')
      .replace(/^を/u, 'お')
      .replace(/は$/u, 'わ')
      .replace(/へ$/u, 'え')
      .replace(/を$/u, 'お');
  }
  return target;
};

const kanaMatchesAt = (reading, index, text, followsKanji) => {
  const target = expectedKana(text, followsKanji);
  if (index + target.length > reading.length) return false;
  const phoneticEquivalents = { は: ['は', 'わ'], へ: ['へ', 'え'], を: ['を', 'お'] };
  return [...target].every((char, offset) =>
    (phoneticEquivalents[char] ?? [char]).includes(reading[index + offset]));
};

const annotateKanjiReadings = (japanese, romaji, lessonId, sentenceNumber) => {
  const { reading, boundaries } = romajiToHiragana(romaji);
  const tokens = getReadingTokens(japanese);
  const jpPunctuation = tokens.filter((token) => token.type === 'punctuation');
  if (jpPunctuation.length !== boundaries.length || jpPunctuation.some((token, index) => token.mark !== boundaries[index].mark)) {
    throw new Error(`Punctuation mismatch in JPD133 lesson ${lessonId}, sentence ${sentenceNumber}: ${japanese} / ${romaji}`);
  }
  const memo = new Map();
  const solve = (tokenIndex, readingIndex) => {
    const key = `${tokenIndex}:${readingIndex}`;
    if (memo.has(key)) return memo.get(key);
    if (tokenIndex === tokens.length) {
      const result = readingIndex === reading.length ? { assignments: [], cost: 0 } : null;
      memo.set(key, result);
      return result;
    }

    const token = tokens[tokenIndex];
    if (token.type === 'punctuation') {
      const boundary = boundaries[token.punctuationIndex];
      const tail = boundary.offset === readingIndex ? solve(tokenIndex + 1, readingIndex) : null;
      memo.set(key, tail);
      return tail;
    }

    if (token.type === 'kana') {
      const followsKanji = tokens[tokenIndex - 1]?.type === 'kanji';
      const normalizedKana = expectedKana(token.text, followsKanji);
      if (!kanaMatchesAt(reading, readingIndex, token.text, followsKanji)) {
        memo.set(key, null);
        return null;
      }
      const tail = solve(tokenIndex + 1, readingIndex + normalizedKana.length);
      const result = tail
        ? { assignments: [{ tokenIndex, reading: '' }, ...tail.assignments], cost: tail.cost }
        : null;
      memo.set(key, result);
      return result;
    }

    const nextToken = tokens[tokenIndex + 1];
    if (!nextToken) {
      const remaining = reading.slice(readingIndex);
      const result = remaining
        ? { assignments: [{ tokenIndex, reading: remaining }], cost: 0 }
        : null;
      memo.set(key, result);
      return result;
    }
    const candidates = [];
    if (nextToken.type === 'punctuation') {
      candidates.push(boundaries[nextToken.punctuationIndex].offset);
    } else if (nextToken.type === 'kana') {
      const maxReadingIndex = Math.min(reading.length, readingIndex + Math.max(24, token.text.length * 5));
      for (let candidate = readingIndex + 1; candidate <= maxReadingIndex; candidate += 1) {
        if (kanaMatchesAt(reading, candidate, nextToken.text, true)) candidates.push(candidate);
      }
    }

    let best = null;
    let bestCost = Infinity;
    for (const candidate of candidates) {
      if (candidate <= readingIndex) continue;
      const tail = solve(tokenIndex + 1, candidate);
      if (!tail) continue;
      const targetLength = [...token.text].length * 2;
      const cost = Math.abs(candidate - readingIndex - targetLength) + tail.cost;
      if (cost < bestCost) {
        bestCost = cost;
        best = { assignments: [{ tokenIndex, reading: reading.slice(readingIndex, candidate) }, ...tail.assignments], cost };
      }
    }
    const result = best;
    memo.set(key, result);
    return result;
  };

  const solution = solve(0, 0);
  if (!solution) {
    throw new Error(`Could not align Kanji readings in JPD133 lesson ${lessonId}, sentence ${sentenceNumber}: ${japanese} / ${romaji} (${reading})`);
  }

  let result = japanese;
  for (const assignment of solution.assignments.filter((item) => item.reading).reverse()) {
    const token = tokens[assignment.tokenIndex];
    const base = japanese.slice(token.start, token.end);
    result = `${result.slice(0, token.start)}[[${base}|${assignment.reading}]]${result.slice(token.end)}`;
  }
  return result;
};

const stripFurigana = (text) => text.replace(/\[\[([^|\]]+)\|[^\]]+\]\]/gu, '$1');

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
    return {
      jp: annotateKanjiReadings(jp, romaji, idText, sentenceIndex + 1),
      vi,
      romaji,
    };
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
  lesson.sentences.map((sentence) => stripFurigana(sentence.jp)).join('\n'),
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
