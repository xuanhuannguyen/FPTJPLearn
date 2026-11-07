import { mkdir, readFile, writeFile } from 'node:fs/promises';
import path from 'node:path';

const root = process.cwd();
const qaSource = path.join(root, 'docs/JPD133/Resource/JPD133_QA_No_Image_Bai_8_11.md');
const vocabSource = path.join(root, 'server/JPLearn.Infrastructure/Data/Imports/vocabulary/jpd133.lessons.json');
const qaText = await readFile(qaSource, 'utf8');
const vocabulary = JSON.parse(await readFile(vocabSource, 'utf8')).lessons.flatMap((lesson) => lesson.items);
const vocabularyByWord = new Map(vocabulary.map((item) => [item.word, item]));

const clean = (value) => value.replaceAll('`', '').trim();
const lessonBlocks = [...qaText.matchAll(/^# JPD133-L(\d+) – Bài (\d+)$/gmu)];
if (lessonBlocks.length !== 4) throw new Error(`Expected four JPD133 QA lessons, found ${lessonBlocks.length}`);

function parseLesson(block, lessonNumber) {
  const sectionBlocks = [...block.matchAll(/^### NI\d+ – ([^\n]+)$/gmu)];
  const sections = sectionBlocks.map((section, index) => {
    const sectionEnd = sectionBlocks[index + 1]?.index ?? block.length;
    const sectionText = block.slice(section.index, sectionEnd);
    const questionBlocks = [...sectionText.matchAll(/^#### Q(\d+)\s*$([\s\S]*?)(?=^#### Q\d+\s*$|^---|(?![\s\S]))/gmu)];
    const questionList = questionBlocks.map((question) => {
      const text = question[2];
      const ja = clean(text.match(/^- Câu hỏi: ([^\n]+)/mu)?.[1] ?? '');
      const vi = clean(text.match(/^- Dịch: ([^\n]+)/mu)?.[1] ?? '');
      const sampleJa = clean(text.match(/- JA: ([^\n]+)/mu)?.[1] ?? '');
      const sampleVi = clean(text.match(/- VI: ([^\n]+)/mu)?.[1] ?? '');
      const vocabNames = clean(text.match(/^- Từ vựng liên quan: ([^\n]+)/mu)?.[1] ?? '').split(/[,、]/u).map((item) => item.trim()).filter(Boolean);
      const grammarRaw = clean(text.match(/^- Ngữ pháp liên quan: ([^\n]+)/mu)?.[1] ?? '');
      const grammarMatch = grammarRaw.match(/JPD133-L(\d\d)/u);
      const grammarIds = grammarMatch ? [`JPD133-L${grammarMatch[1]}-G001`] : [`JPD133-L${String(lessonNumber).padStart(2, '0')}-G001`];
      const relatedVocabulary = vocabNames.map((name) => {
        const item = vocabularyByWord.get(name);
        return item
          ? { word: item.word, reading: item.reading, meaning: item.meaning }
          : { word: name, reading: name, meaning: name };
      });
      return {
        questionId: `jpd133_l${lessonNumber}_${section[0].match(/NI\d+/u)[0].toLowerCase()}_q${String(question[1]).padStart(2, '0')}`,
        order: Number(question[1]),
        question: { ja, vi },
        answerType: 'short_answer',
        sampleAnswers: [{ ja: sampleJa, vi: sampleVi }],
        grammarIds,
        relatedVocabulary,
        explanation: clean(text.match(/^- Giải thích: ([^\n]+)/mu)?.[1] ?? ''),
        tips: [clean(text.match(/^- Mẹo: ([^\n]+)/mu)?.[1] ?? '')].filter(Boolean),
        commonMistakes: [clean(text.match(/^- Lỗi thường gặp: ([^\n]+)/mu)?.[1] ?? '')].filter(Boolean),
      };
    });
    return {
      sectionId: `jpd133_l${lessonNumber}_${section[0].match(/NI\d+/u)[0].toLowerCase()}`,
      sectionTitle: section[1],
      sectionViTitle: section[1],
      sectionGoal: `Luyện trả lời câu hỏi nói Bài ${lessonNumber}: ${section[1]}.`,
      questionList,
    };
  });
  return { sections };
}

const overview = (lessonNumber) => ({
  shortSummary: `Bài ${lessonNumber} luyện phản xạ trả lời ngắn theo từ vựng và ngữ pháp JPD133.`,
  studentCanDo: [`Trả lời câu hỏi chủ đề Bài ${lessonNumber}.`, 'Dùng câu trả lời tiếng Nhật ngắn, đúng mẫu.'],
  mainSkills: ['Nghe từ khóa câu hỏi.', 'Chọn mẫu câu và từ vựng phù hợp để trả lời.'],
  mainGrammarFocus: [`JPD133-L${String(lessonNumber).padStart(2, '0')}-G001`],
  examTipSummary: 'Nghe từ khóa, trả lời đủ thông tin và ưu tiên mẫu câu đã học.',
});

const noImageLessons = [];
const withImageLessons = [];
for (let index = 0; index < lessonBlocks.length; index += 1) {
  const lessonNumber = Number(lessonBlocks[index][1]);
  const start = lessonBlocks[index].index + lessonBlocks[index][0].length;
  const end = lessonBlocks[index + 1]?.index ?? qaText.length;
  const parsed = parseLesson(qaText.slice(start, end), lessonNumber);
  const base = { courseCode: 'jpd133', lessonNumber, lessonTitle: `Bài ${lessonNumber} - JPD133`, dataPurpose: 'oral_exam_practice_web' };
  noImageLessons.push({ ...base, questionMode: 'NO_IMAGE', lessonOverview: overview(lessonNumber), sections: parsed.sections });
  withImageLessons.push({
    ...base,
    questionMode: 'WITH_IMAGE',
    lessonOverview: overview(lessonNumber),
    pictureSets: [{
      pictureId: `jpd133_l${lessonNumber}_cover`,
      pictureTitle: `Ảnh bìa tạm thời JPD133 Bài ${lessonNumber}`,
      imageUrl: '/images/course-cards/jpd133-bg.webp',
      questions: parsed.sections.flatMap((section) => section.questionList),
    }],
  });
}

const writeQa = async (lessons, suffix) => {
  for (const lesson of lessons) {
    const sourceDir = path.join(root, 'server/JPLearn.Infrastructure/Data/Imports/speaking/qa/jpd133');
    await mkdir(sourceDir, { recursive: true });
    await writeFile(path.join(sourceDir, `lesson${lesson.lessonNumber}_${suffix}.json`), `${JSON.stringify(lesson, null, 2)}\n`, 'utf8');
  }
};
await writeQa(noImageLessons, 'no_image');
console.log(`Generated ${noImageLessons.length} no-image QA lessons.`);
