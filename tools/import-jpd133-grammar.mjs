import { readFile, writeFile } from 'node:fs/promises';

const sourcePath = 'docs/JPD133/Resource/JPD133_Ngu_phap_Bai_8_11_chi_tiet_co_bai_tap.md';
const targetPath = 'server/JPLearn.Infrastructure/Data/Imports/grammar_jpd133.json';
const markdown = await readFile(sourcePath, 'utf8');

const clean = (value) => value.replaceAll('`', '').replaceAll('**', '').replace(/\s+$/u, '').trim();
const patternMatches = [...markdown.matchAll(/^### (JPD133-L(\d+)-G(\d+)) – (.+)$/gmu)];
const lessons = new Map();

for (const match of patternMatches) {
  const [, code, lessonNumberText, patternNumberText, title] = match;
  const lessonNumber = Number(lessonNumberText);
  const start = match.index + match[0].length;
  const next = patternMatches.find((candidate) => candidate.index > match.index)?.index ?? markdown.length;
  const block = markdown.slice(start, next);
  const fields = {};
  for (const field of ['Mẫu câu', 'Ý nghĩa', 'Cấu trúc', 'Điều kiện / phạm vi dùng', 'Biến đổi / cách tạo', 'Trợ từ liên quan', 'Mẫu dễ nhầm', 'Lỗi thường gặp']) {
    const fieldMatch = block.match(new RegExp(`\\| \\*\\*${field}\\*\\* \\| ([^\\n]+)`));
    fields[field] = fieldMatch ? clean(fieldMatch[1].replace(/\s*\|\s*$/u, '')) : '';
  }

  const examples = [];
  const exampleRegex = /\*\*(?:Ví dụ gốc trong tài liệu|Ví dụ bổ sung \(biên soạn theo đúng mẫu\)):\*\*[ \t]*\n+`([^`]+)`[ \t]*\n+\*\*Cách đọc:\*\* ([^\n]+)[ \t]*\n+\*\*Dịch:\*\* ([^\n]+)/gu;
  for (const example of block.matchAll(exampleRegex)) {
    examples.push({ japanese: clean(example[1]), reading: clean(example[2]), meaning: clean(example[3]), orderIndex: examples.length + 1 });
  }

  const lesson = lessons.get(lessonNumber) ?? {
    lessonNumber,
    title: `JPD133 Bài ${lessonNumber}`,
    description: '',
    accessTier: lessonNumber === 8 ? 'free' : 'premium',
    packageCode: 'jpd133',
    orderIndex: lessonNumber,
    patterns: [],
  };
  lesson.patterns.push({
    pattern: fields['Mẫu câu'],
    title,
    meaning: fields['Ý nghĩa'],
    structure: fields['Cấu trúc'],
    usageScope: fields['Điều kiện / phạm vi dùng'],
    formation: fields['Biến đổi / cách tạo'],
    notes: [fields['Trợ từ liên quan'], fields['Mẫu dễ nhầm'], fields['Lỗi thường gặp']].filter(Boolean).join(' '),
    orderIndex: Number(patternNumberText),
    examples,
    exercises: [],
    sourceCode: code,
  });
  lessons.set(lessonNumber, lesson);
}

const exerciseSections = [...markdown.matchAll(/^# Bài tập 第(\d+)課[^\n]*$/gmu)];
for (const section of exerciseSections) {
  const lessonNumber = Number(section[1]);
  const start = section.index + section[0].length;
  const end = exerciseSections.find((candidate) => candidate.index > section.index)?.index ?? markdown.length;
  const block = markdown.slice(start, end);
  const lesson = lessons.get(lessonNumber);
  if (!lesson) continue;
  const questionHeadings = [...block.matchAll(/^### Câu (\d+)\s*$/gmu)];
  for (let questionIndex = 0; questionIndex < questionHeadings.length; questionIndex += 1) {
    const question = questionHeadings[questionIndex];
    const questionStart = question.index + question[0].length;
    const questionEnd = questionHeadings[questionIndex + 1]?.index ?? block.length;
    const questionBlock = block.slice(questionStart, questionEnd);
    const sectionName = block.slice(0, question.index).match(/^## ([A-E])\./gmu)?.at(-1)?.match(/([A-E])/u)?.[1] ?? 'E';
    const prompt = clean(questionBlock.match(/\*\*Đề:\*\*\s*([^\n]+)/u)?.[1] ?? questionBlock.split('\n').map((line) => line.trim()).find(Boolean) ?? '');
    const answer = clean(
      questionBlock.match(/\*\*Đáp án tham khảo:\*\* `?([^`\n]+)`?/u)?.[1]
        ?? questionBlock.match(/\*\*Đáp án:\*\* `([^`\n]+)`/u)?.[1]
        ?? '',
    );
    const options = [...questionBlock.matchAll(/^- ([A-D])\. (.+)$/gmu)].map((item) => clean(item[2]));
    const arrangeOptions = clean(questionBlock.match(/Các từ: `([^`]+)`/u)?.[1] ?? '').split(/\s*\/\s*/u).filter(Boolean);
    const answerLetter = questionBlock.match(/\*\*Đáp án:\*\* ([A-D])/u)?.[1];
    const expectedAnswer = answer || (answerLetter ? options['ABCD'.indexOf(answerLetter)] ?? '' : '');
    const isArrange = sectionName === 'B';
    const isJapanesePrompt = Boolean(questionBlock.match(/^- [A-D]\. /m));
    const type = isArrange ? 'arrange' : sectionName === 'C' ? 'vi_to_ja' : isJapanesePrompt ? 'vi_to_ja' : 'vi_to_ja';
    const pattern = lesson.patterns.find((item) => expectedAnswer && item.examples.some((example) => example.japanese.includes(expectedAnswer))) ?? lesson.patterns[0];
    pattern.exercises.push({
      exerciseType: type,
      prompt: isArrange ? clean(questionBlock.match(/Các từ: `([^`]+)`/u)?.[1] ?? prompt) : prompt,
      expectedAnswer,
      acceptableAnswers: [],
      hint: '',
      explanation: clean(questionBlock.match(/\*\*(?:Giải thích):\*\* ([^\n]+)/u)?.[1] ?? ''),
      options: isArrange ? arrangeOptions : options,
      correctOrder: isArrange
        ? arrangeOptions.slice().sort((a, b) => expectedAnswer.indexOf(a) - expectedAnswer.indexOf(b))
        : [],
      orderIndex: pattern.exercises.length + 1,
    });
  }
}

const result = {
  courseCode: 'jpd133',
  level: 'N5',
  lessons: [...lessons.values()].sort((a, b) => a.lessonNumber - b.lessonNumber).map((lesson) => ({
    ...lesson,
    description: `JPD133 Bài ${lesson.lessonNumber} – ngữ pháp và bài tập.`,
    patterns: lesson.patterns.sort((a, b) => a.orderIndex - b.orderIndex),
  })),
};
await writeFile(targetPath, `${JSON.stringify(result, null, 2)}\n`, 'utf8');
console.log(`Imported ${result.lessons.length} lessons, ${result.lessons.flatMap((lesson) => lesson.patterns).length} patterns and ${result.lessons.flatMap((lesson) => lesson.patterns).flatMap((pattern) => pattern.exercises).length} exercises.`);
