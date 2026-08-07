import { mkdir, readFile, writeFile } from 'node:fs/promises';
import path from 'node:path';

const root = process.cwd();
const sourcePath = path.join(root, 'docs/JPD133/Resource/JPD133_QA_CoTranh/JPD133_Ke_Hoach_QA_Co_Tranh_Chi_Tiet.md');
const markdown = await readFile(sourcePath, 'utf8');
const lessonMatches = [...markdown.matchAll(/^# 第(9|10|11)課$/gmu)];
const pictureMatches = [...markdown.matchAll(/^# P(\d+) – ([^\n]+)$/gmu)];
if (lessonMatches.length !== 3 || pictureMatches.length !== 25) {
  throw new Error(`Expected headings for lessons 9–11 and 25 source pictures, found ${lessonMatches.length} headings and ${pictureMatches.length} pictures.`);
}

const clean = (value) => value.replaceAll('**', '').replaceAll('`', '').trim();
const grammarId = (lessonNumber) => `JPD133-L${String(lessonNumber).padStart(2, '0')}-G001`;
const pictureCounters = new Map();
const pictureSetsByLesson = new Map([[8, []], [9, []], [10, []], [11, []]]);

for (let index = 0; index < pictureMatches.length; index += 1) {
  const picture = pictureMatches[index];
  const lessonMatch = lessonMatches.filter((candidate) => candidate.index < picture.index).at(-1);
  const lessonNumber = lessonMatch ? Number(lessonMatch[1]) : 8;
  const pictureNumber = (pictureCounters.get(lessonNumber) ?? 0) + 1;
  pictureCounters.set(lessonNumber, pictureNumber);
  const start = picture.index + picture[0].length;
  const end = pictureMatches[index + 1]?.index ?? markdown.length;
  const block = markdown.slice(start, end);
  const questionMatches = [...block.matchAll(/^# ❓ Câu (\d+)$/gmu)];
  if (questionMatches.length !== 5) throw new Error(`Expected five questions for lesson ${lessonNumber}, picture ${picture[1]}.`);

  const questions = questionMatches.map((question, questionIndex) => {
    const qStart = question.index + question[0].length;
    const qEnd = questionMatches[questionIndex + 1]?.index ?? block.length;
    const qBlock = block.slice(qStart, qEnd);
    const questionJa = clean(qBlock.match(/## Câu hỏi\s*\n\s*\*\*([^\n]+)\*\*/mu)?.[1] ?? '');
    const questionReading = clean(qBlock.match(/### Hiragana\s*\n\s*> ([^\n]+)/mu)?.[1] ?? '');
    const questionVi = clean(qBlock.match(/### Nghĩa\s*\n\s*> ([^\n]+)/mu)?.[1] ?? '');
    const answerJa = clean(qBlock.match(/## 💬 Gợi ý trả lời\s*\n\s*\*\*([^\n]+)\*\*/mu)?.[1] ?? '');
    const answerReading = clean(qBlock.match(/## 💬 Gợi ý trả lời[\s\S]*?### Hiragana\s*\n\s*> ([^\n]+)/mu)?.[1] ?? '');
    const answerVi = clean(qBlock.match(/## 💬 Gợi ý trả lời[\s\S]*?### Nghĩa\s*\n\s*> ([^\n]+)/mu)?.[1] ?? '');
    const tips = [...qBlock.matchAll(/^- (.+)$/gmu)].map((item) => clean(item[1])).filter((item) => !item.startsWith('Dùng ') || item.length > 0);
    const vocabStart = qBlock.indexOf('## 📖 Từ vựng liên quan');
    const vocabBlock = vocabStart >= 0 ? qBlock.slice(vocabStart) : '';
    const relatedVocabulary = [...vocabBlock.matchAll(/^\|\s*([^|]+?)\s*\|\s*([^|]+?)\s*\|\s*([^|]+?)\s*\|$/gmu)]
      .filter((item) => !/^Tiếng Nhật$|^-+$|^Japanese$/u.test(item[1].trim()))
      .map((item) => ({ word: clean(item[1]), reading: clean(item[2]), meaning: clean(item[3]) }));
    if (!questionJa || !questionReading || !questionVi || !answerJa || !answerReading || !answerVi || relatedVocabulary.length === 0) {
      throw new Error(`Incomplete picture QA: lesson ${lessonNumber}, picture ${picture[1]}, question ${question[1]}.`);
    }
    return {
      questionId: `jpd133_l${lessonNumber}_p${String(pictureNumber).padStart(2, '0')}_q${String(question[1]).padStart(2, '0')}`,
      order: Number(question[1]),
      question: { ja: questionJa, vi: questionVi },
      questionReading,
      answerType: 'picture_short_answer',
      sampleAnswers: [{ ja: answerJa, vi: answerVi, reading: answerReading }],
      grammarIds: [grammarId(lessonNumber)],
      relatedVocabulary,
      explanation: `Đọc câu hỏi và đối chiếu thông tin trong tranh Bài ${lessonNumber}.`,
      tips,
      commonMistakes: [],
    };
  });

  pictureSetsByLesson.get(lessonNumber).push({
    pictureId: `jpd133_l${lessonNumber}_p${String(pictureNumber).padStart(2, '0')}`,
    pictureTitle: clean(picture[2]),
    imageUrl: `/data/speaking/jpd133/qa/lesson${lessonNumber}_tranh${pictureNumber}.png`,
    questions,
  });
}

const outputDir = path.join(root, 'server/JPLearn.Infrastructure/Data/Imports/speaking/qa/jpd133');
await mkdir(outputDir, { recursive: true });
for (const [lessonNumber, pictureSets] of pictureSetsByLesson) {
  const output = {
    courseCode: 'jpd133',
    lessonNumber,
    lessonTitle: `Bài ${lessonNumber} - JPD133 Q&A Có Tranh`,
    questionMode: 'WITH_IMAGE',
    dataPurpose: 'oral_exam_practice_web',
    lessonOverview: {
      shortSummary: `Luyện phản xạ nhìn tranh và trả lời câu hỏi JPD133 Bài ${lessonNumber}.`,
      studentCanDo: ['Nhận diện thông tin chính trong tranh.', 'Trả lời bằng câu tiếng Nhật ngắn và đúng mẫu.'],
      mainSkills: ['Quan sát người, vật, địa điểm và hành động.', 'Dùng từ vựng và mẫu câu theo bài.'],
      mainGrammarFocus: [grammarId(lessonNumber)],
      examTipSummary: 'Quan sát toàn bộ tranh trước khi trả lời; ưu tiên câu ngắn, đủ chủ ngữ và động từ.',
    },
    pictureSets,
  };
  await writeFile(path.join(outputDir, `lesson${lessonNumber}_with_image.json`), `${JSON.stringify(output, null, 2)}\n`, 'utf8');
}
console.log(`Imported ${pictureMatches.length} pictures and ${pictureMatches.length * 5} questions. Source currently provides 7 pictures for Bài 8 and 6 pictures for each of Bài 9–11.`);
