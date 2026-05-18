import { mkdir, readFile, rm, writeFile } from 'node:fs/promises';
import path from 'node:path';

const root = process.cwd();
const outRoot = path.join(root, 'client', 'public', 'data');

const readJson = async (relativePath) =>
  JSON.parse(await readFile(path.join(root, relativePath), 'utf8'));

const writeJson = async (relativePath, value) => {
  const filePath = path.join(outRoot, relativePath);
  await mkdir(path.dirname(filePath), { recursive: true });
  await writeFile(filePath, `${JSON.stringify(value, null, 2)}\n`, 'utf8');
};

const segment = (courseCode) => (courseCode.toLowerCase().includes('113') ? '1113' : '1123');
const guid = (prefix, courseCode, group, number) =>
  `${prefix}-${segment(courseCode)}-${group}-0000-${String(number).padStart(12, '0')}`;

const normalizeTier = (tier) => (tier?.trim().toLowerCase() || 'free');
const courseOrder = (code) => (code.toLowerCase() === 'jpd113' ? 1 : 2);

const toPlainText = (text) => text.replace(/\[\[([^|\]]+)\|[^\]]+\]\]/g, '$1').replaceAll(' ', '');
const escapeHtml = (text) =>
  text
    .replaceAll('&', '&amp;')
    .replaceAll('<', '&lt;')
    .replaceAll('>', '&gt;')
    .replaceAll('"', '&quot;')
    .replaceAll("'", '&#39;');
const toRubyHtml = (text) =>
  escapeHtml(text).replace(/\[\[([^|\]]+)\|([^\]]+)\]\]/g, (_, base, reading) =>
    `<ruby>${escapeHtml(base)}<rt>${escapeHtml(reading)}</rt></ruby>`,
  );

const shuffleStable = (items) =>
  [...items].sort((a, b) => {
    const av = [...a].reduce((sum, ch) => sum + ch.charCodeAt(0), 0);
    const bv = [...b].reduce((sum, ch) => sum + ch.charCodeAt(0), 0);
    return av - bv || a.localeCompare(b);
  });

const formatWithFurigana = (japanese, reading = '') => {
  if (!japanese.trim()) return '';
  if (!reading.trim() || japanese === reading) return japanese;
  return /[\u4e00-\u9faf]/.test(japanese) ? `${japanese} (${reading})` : japanese;
};

const smartSplitWithReading = (text, reading) => {
  let current = text.trim().replace(/[。！？.]$/u, '');
  let currentReading = (reading || '').trim().replace(/[。！？.]$/u, '');
  const particles = ['は', 'の', 'に', 'へ', 'を', 'が', 'と', 'も', 'から', 'まで', 'です', 'でした', 'じゃありません'];
  const parts = [];

  while (current) {
    let bestIndex = -1;
    let bestParticle = '';
    for (const particle of particles) {
      const idx = current.indexOf(particle);
      if (idx !== -1 && (bestIndex === -1 || idx < bestIndex)) {
        bestIndex = idx;
        bestParticle = particle;
      }
    }

    if (bestIndex === -1) {
      parts.push(formatWithFurigana(current, currentReading));
      break;
    }

    if (bestIndex > 0) {
      const japanesePart = current.slice(0, bestIndex);
      const readingIndex = currentReading.indexOf(bestParticle);
      if (currentReading && readingIndex > 0) {
        parts.push(formatWithFurigana(japanesePart, currentReading.slice(0, readingIndex)));
        currentReading = currentReading.slice(readingIndex + bestParticle.length);
      } else {
        parts.push(japanesePart);
        if (readingIndex === 0) currentReading = currentReading.slice(bestParticle.length);
      }
    }

    parts.push(bestParticle);
    current = current.slice(bestIndex + bestParticle.length);
  }

  return parts.filter((part) => part.trim());
};

async function generateVocabulary() {
  const imports = await Promise.all(
    ['jpd113', 'jpd123'].map((code) => readJson(`server/JPLearn.Infrastructure/Data/Imports/vocabulary/${code}.lessons.json`)),
  );
  const allItems = [];
  const courses = [];

  for (const importFile of imports.sort((a, b) => courseOrder(a.courseCode) - courseOrder(b.courseCode))) {
    const lessons = importFile.lessons.map((seed) => {
      const lessonId = guid('66666666', importFile.courseCode, '0000', seed.id + 100);
      const items = seed.items.map((item, index) => {
        const accessTier = normalizeTier(item.accessTierOverride || seed.accessTier);
        const packageCode = item.packageCodeOverride || seed.packageCode;
        return {
          id: item.id || guid('66666666', importFile.courseCode, '0000', seed.id * 1000 + index + 1),
          lessonId,
          courseCode: importFile.courseCode,
          word: item.word,
          reading: item.reading,
          wordType: item.wordType,
          meaning: item.meaning,
          exampleJapanese: item.exampleJapanese,
          exampleReading: item.exampleReading,
          exampleMeaning: item.exampleMeaning,
          notes: item.notes,
          accessTier,
          packageCode,
          isLocked: accessTier !== 'free',
          isLearned: false,
          flashcardPracticeCount: 0,
          multipleChoicePracticeCount: 0,
          typingPracticeCount: 0,
          orderIndex: item.orderIndex ?? index + 1,
        };
      });
      allItems.push(...items);

      const lesson = {
        id: lessonId,
        courseCode: importFile.courseCode,
        lessonNumber: seed.id,
        title: seed.title,
        description: seed.description,
        accessTier: normalizeTier(seed.accessTier),
        packageCode: seed.packageCode,
        isLocked: normalizeTier(seed.accessTier) !== 'free',
        wordCount: items.length,
        learnedCount: 0,
        practicedCount: 0,
      };

      return { lesson, items };
    });

    courses.push({
      id: importFile.courseCode === 'jpd113' ? '66666666-1113-0000-0000-000000000001' : '66666666-1123-0000-0000-000000000001',
      code: importFile.courseCode,
      title: importFile.title || importFile.courseCode.toUpperCase(),
      description: importFile.description,
      lessonCount: lessons.length,
      wordCount: lessons.reduce((sum, item) => sum + item.items.length, 0),
      learnedCount: 0,
      practicedCount: 0,
    });

    await writeJson(`vocabulary/${importFile.courseCode}/lessons.json`, lessons.map((item) => item.lesson));
    for (const detail of lessons) {
      await writeJson(`vocabulary/${importFile.courseCode}/lessons/${detail.lesson.id}.json`, detail);
    }
  }

  await writeJson('vocabulary/courses.json', courses);
  await writeJson('vocabulary/items.json', allItems);
}

async function generateSpeaking() {
  const imports = await Promise.all(
    ['jpd113', 'jpd123'].map((code) => readJson(`server/JPLearn.Infrastructure/Data/Imports/speaking/${code}.lessons.json`)),
  );
  const courses = [];

  for (const importFile of imports.sort((a, b) => courseOrder(a.courseCode) - courseOrder(b.courseCode))) {
    const accessTier = normalizeTier(importFile.accessTier);
    const lessons = importFile.lessons.map((seed) => {
      const lessonId = guid('88888888', importFile.courseCode, '0000', seed.id + 100);
      const lesson = {
        id: lessonId,
        courseCode: importFile.courseCode,
        lessonNumber: seed.id,
        title: seed.title,
        description: `${seed.topic} - ${seed.subtitle}`,
        accessTier,
        packageCode: importFile.packageCode,
        isLocked: accessTier !== 'free',
        sentenceCount: seed.sentences.length,
      };
      const sentences = seed.sentences.map((sentence, index) => ({
        id: guid('88888888', importFile.courseCode, '0000', seed.id * 1000 + index + 1),
        lessonId,
        sentenceNumber: index + 1,
        plainText: toPlainText(sentence.jp),
        romaji: sentence.romaji,
        contentHtml: toRubyHtml(sentence.jp),
        meaningVi: sentence.vi,
        orderIndex: index + 1,
      }));
      return { lesson, sentences };
    });

    courses.push({
      id: importFile.courseCode === 'jpd113' ? '88888888-1113-0000-0000-000000000001' : '88888888-1123-0000-0000-000000000001',
      code: importFile.courseCode,
      title: importFile.courseCode.toUpperCase(),
      description: `Bài đọc luyện nói ${importFile.courseCode.toUpperCase()}.`,
      accessTier,
      packageCode: importFile.packageCode,
      isLocked: accessTier !== 'free',
      lessonCount: lessons.length,
      sentenceCount: lessons.reduce((sum, item) => sum + item.sentences.length, 0),
    });

    await writeJson(`speaking/${importFile.courseCode}/lessons.json`, lessons.map((item) => item.lesson));
    for (const detail of lessons) {
      await writeJson(`speaking/${importFile.courseCode}/lessons/${detail.lesson.id}.json`, detail);
    }
  }

  await writeJson('speaking/courses.json', courses);
}

async function generateKanji() {
  const courses = new Map();
  const kanjiSeedIds = await loadKanjiSeedIds();

  for (const courseCode of ['jpd113', 'jpd123']) {
    const core = await readJson(`material/KANJI/${courseCode}_core.json`);
    const vocab = await readJson(`material/KANJI/${courseCode}_vocab.json`);
    for (const seed of core) {
      const level = seed.level || 'N5';
      const lessonId = `kanji-${courseCode}-${level.toLowerCase()}-${seed.lessonNumber}`;
      const accessTier = seed.accessTier || (courseCode === 'jpd113' && seed.lessonNumber === 1 ? 'free' : 'premium');
      const lesson = {
        id: lessonId,
        level,
        lessonNumber: seed.lessonNumber,
        title: seed.lessonTitle,
        description: `${seed.lessonTitle} - ${courseCode.toUpperCase()} Kanji ${level}.`,
        accessTier,
        packageCode: `kanji_${courseCode}`,
        orderIndex: seed.lessonNumber,
        kanjiCount: seed.kanjiItems.length,
        vocabularyCount: 0,
        isLocked: normalizeTier(accessTier) !== 'free',
      };
      const vocabularyItems = (vocab.find((item) => item.lessonNumber === seed.lessonNumber)?.vocabulary || []).map((item, index) => ({
        id: `${lessonId}-vocab-${index + 1}`,
        lessonId,
        level,
        word: item.word,
        reading: item.reading,
        meaning: item.meaning,
        exampleJapanese: item.exampleJapanese,
        exampleReading: item.exampleReading,
        exampleMeaning: item.exampleMeaning,
      }));
      lesson.vocabularyCount = vocabularyItems.length;
      const kanjiItems = seed.kanjiItems.map((item, index) => {
        const seedId = kanjiSeedIds.get(seed.lessonNumber)?.[index];
        if (seedId && seedId.character !== item.character) {
          throw new Error(`Kanji seed ID mismatch in lesson ${seed.lessonNumber}: expected ${item.character}, got ${seedId.character}`);
        }

        return {
          id: seedId?.id || `${lessonId}-kanji-${index + 1}`,
          lessonId,
          level,
          character: item.character,
          hanViet: item.hanViet,
          meaning: item.meaning,
          strokeCount: item.strokeCount,
          kunReading: item.kunReading || '',
          onReading: item.onReading || '',
          mnemonic: item.mnemonic || '',
          strokeSvg: item.strokeSvg,
          strokeDataJson: item.strokeDataJson,
          componentMapJson: item.componentMapJson,
          orderIndex: item.orderIndex ?? index + 1,
        };
      });

      if (!courses.has(courseCode)) courses.set(courseCode, []);
      courses.get(courseCode).push({ lesson, kanjiItems, vocabularyItems });
    }
  }

  const courseStats = [];
  for (const [courseCode, details] of courses.entries()) {
    details.sort((a, b) => a.lesson.orderIndex - b.lesson.orderIndex);
    await writeJson(`kanji/${courseCode}/lessons.json`, details.map((item) => item.lesson));
    for (const detail of details) {
      await writeJson(`kanji/${courseCode}/lessons/${detail.lesson.id}.json`, detail);
    }
    courseStats.push({
      level: 'N5',
      courseCode,
      totalLessons: details.length,
      totalKanji: details.reduce((sum, item) => sum + item.kanjiItems.length, 0),
      totalVocabulary: details.reduce((sum, item) => sum + item.vocabularyItems.length, 0),
      progressPercentage: 0,
    });
  }
  await writeJson('kanji/levels.json', courseStats.sort((a, b) => courseOrder(a.courseCode) - courseOrder(b.courseCode)));
}

async function generateGrammar() {
  const imports = await Promise.all(
    ['jpd113', 'jpd123'].map((code) => readJson(`server/JPLearn.Infrastructure/Data/Imports/grammar_${code}.json`)),
  );
  const byLevel = new Map();
  const allPatterns = [];
  const exercisesByPattern = new Map();

  for (const importFile of imports.sort((a, b) => courseOrder(a.courseCode) - courseOrder(b.courseCode))) {
    const level = importFile.level || 'N5';
    if (!byLevel.has(level)) byLevel.set(level, []);

    for (const lessonSeed of importFile.lessons) {
      const lessonId = guid('77777777', importFile.courseCode, '0000', lessonSeed.lessonNumber + 100);
      const accessTier = normalizeTier(lessonSeed.accessTier);
      const lesson = {
        id: lessonId,
        level,
        courseCode: importFile.courseCode,
        lessonNumber: lessonSeed.lessonNumber,
        title: lessonSeed.title,
        description: lessonSeed.description,
        patternCount: lessonSeed.patterns.length,
        accessTier,
        packageCode: lessonSeed.packageCode,
        isLocked: accessTier !== 'free',
        inStudyCount: 0,
        masteredCount: 0,
        dueCount: 0,
      };

      const patterns = lessonSeed.patterns.map((patternSeed, patternIndex) => {
        const patternId = guid('77777777', importFile.courseCode, '0001', lessonSeed.lessonNumber * 1000 + patternIndex + 1);
        const examples = (patternSeed.examples || []).map((example, exampleIndex) => ({
          id: guid('77777777', importFile.courseCode, '0002', lessonSeed.lessonNumber * 100000 + patternIndex * 1000 + exampleIndex + 1),
          japanese: example.japanese,
          reading: example.reading,
          meaning: example.meaning,
          note: example.note,
          orderIndex: example.orderIndex ?? exampleIndex + 1,
        }));

        const exercises = [];
        let order = 0;
        examples.forEach((example, exampleIndex) => {
          if (example.meaning) {
            exercises.push({
              id: guid('77777777', importFile.courseCode, '0003', lessonSeed.lessonNumber * 100000 + patternIndex * 1000 + 10000 + exampleIndex + 1),
              patternId,
              exerciseType: 'ja_to_vi',
              prompt: example.japanese,
              promptReading: example.reading,
              expectedAnswer: example.meaning,
              hint: patternSeed.pattern,
              explanation: patternSeed.notes,
              orderIndex: ++order,
            });
          }
          if (example.japanese) {
            exercises.push({
              id: guid('77777777', importFile.courseCode, '0003', lessonSeed.lessonNumber * 100000 + patternIndex * 1000 + 20000 + exampleIndex + 1),
              patternId,
              exerciseType: 'vi_to_ja',
              prompt: example.meaning,
              expectedAnswer: example.japanese,
              acceptableAnswers: example.reading ? [example.reading] : [],
              hint: patternSeed.formation,
              explanation: patternSeed.notes,
              orderIndex: ++order,
            });
          }
          const options = smartSplitWithReading(example.japanese || '', example.reading);
          if (options.length >= 2) {
            exercises.push({
              id: guid('77777777', importFile.courseCode, '0003', lessonSeed.lessonNumber * 100000 + patternIndex * 1000 + 30000 + exampleIndex + 1),
              patternId,
              exerciseType: 'arrange',
              prompt: example.meaning,
              expectedAnswer: options.join(''),
              optionsJson: JSON.stringify(shuffleStable(options)),
              options: shuffleStable(options),
              correctOrderJson: JSON.stringify(options),
              correctOrder: options,
              orderIndex: ++order,
            });
          }
        });

        (patternSeed.exercises || []).forEach((exerciseSeed, exerciseIndex) => {
          const options = exerciseSeed.options || [];
          const correctOrder = exerciseSeed.correctOrder || [];
          exercises.push({
            id: guid('77777777', importFile.courseCode, '0003', lessonSeed.lessonNumber * 100000 + patternIndex * 1000 + 40000 + exerciseIndex + 1),
            patternId,
            exerciseType: normalizeTier(exerciseSeed.exerciseType || 'vi_to_ja'),
            prompt: exerciseSeed.prompt,
            promptReading: exerciseSeed.promptReading,
            expectedAnswer: exerciseSeed.expectedAnswer || '',
            acceptableAnswers: exerciseSeed.acceptableAnswers || [],
            acceptableAnswersJson: exerciseSeed.acceptableAnswers ? JSON.stringify(exerciseSeed.acceptableAnswers) : undefined,
            hint: exerciseSeed.hint,
            explanation: exerciseSeed.explanation,
            templateText: exerciseSeed.templateText,
            options,
            optionsJson: options.length ? JSON.stringify(options) : undefined,
            correctOrder,
            correctOrderJson: correctOrder.length ? JSON.stringify(correctOrder) : undefined,
            starPosition: exerciseSeed.starPosition,
            starAnswer: exerciseSeed.starAnswer,
            orderIndex: exerciseSeed.orderIndex ?? ++order,
          });
        });

        const pattern = {
          id: patternId,
          lessonId,
          level,
          courseCode: importFile.courseCode,
          pattern: patternSeed.pattern,
          title: patternSeed.title,
          meaning: patternSeed.meaning,
          structure: patternSeed.structure,
          accessTier,
          packageCode: lessonSeed.packageCode,
          isLocked: accessTier !== 'free',
          isInStudy: false,
          progress: null,
          usageScope: patternSeed.usageScope,
          formation: patternSeed.formation,
          notes: patternSeed.notes,
          tagsJson: patternSeed.tagsJson,
          examples,
          exercises,
        };

        exercisesByPattern.set(patternId, exercises);
        allPatterns.push(pattern);
        return pattern;
      });

      const detail = { lesson, patterns };
      byLevel.get(level).push(detail);
      await writeJson(`grammar/${level}/lessons/${lessonId}.json`, detail);
    }
  }

  const levelSummaries = [];
  for (const [level, details] of byLevel.entries()) {
    details.sort((a, b) => courseOrder(a.lesson.courseCode) - courseOrder(b.lesson.courseCode) || a.lesson.lessonNumber - b.lesson.lessonNumber);
    await writeJson(`grammar/${level}/lessons.json`, details.map((item) => item.lesson));
    const courses = new Set(details.map((item) => item.lesson.courseCode));
    for (const courseCode of courses) {
      const courseLessons = details.filter((item) => item.lesson.courseCode === courseCode);
      const patterns = courseLessons.flatMap((item) => item.patterns);
      levelSummaries.push({
        level,
        courseCode,
        lessonCount: courseLessons.length,
        patternCount: patterns.length,
        freeCount: patterns.filter((item) => item.accessTier === 'free').length,
        premiumCount: patterns.filter((item) => item.accessTier !== 'free').length,
        inStudyCount: 0,
        masteredCount: 0,
        dueCount: 0,
      });
    }
  }

  await writeJson('grammar/levels.json', levelSummaries);
  await writeJson('grammar/patterns.json', allPatterns);
  for (const [patternId, exercises] of exercisesByPattern.entries()) {
    await writeJson(`grammar/exercises/${patternId}.json`, exercises);
  }
}

const normalizeTopic = (topic) => topic.toLowerCase().trim().replaceAll(' ', '_');
const normalizeQuestionType = (type) => (type === 'reading' ? 'passage' : 'standalone');

async function loadKanjiSeedIds() {
  const source = await readFile(path.join(root, 'server', 'JPLearn.Infrastructure', 'Data', 'Seed', 'KanjiSeedData.cs'), 'utf8');
  const idsByLesson = new Map();
  const lessonRegex = /var lesson(\d+)Id = Guid\.Parse\("([^"]+)"\);/g;
  const lessonMatches = [...source.matchAll(lessonRegex)];

  for (let index = 0; index < lessonMatches.length; index += 1) {
    const lessonNumber = Number(lessonMatches[index][1]);
    const start = lessonMatches[index].index ?? 0;
    const end = index + 1 < lessonMatches.length ? lessonMatches[index + 1].index ?? source.length : source.length;
    const section = source.slice(start, end);
    const kanjiIds = [...section.matchAll(/new KanjiItem\s*\{\s*[\s\S]*?Id = Guid\.Parse\("([^"]+)"\),[\s\S]*?Character = "([^"]+)"/g)]
      .map((match) => ({ id: match[1], character: match[2] }));
    idsByLesson.set(lessonNumber, kanjiIds);
  }

  return idsByLesson;
}

async function generateExam() {
  const imports = await Promise.all(
    ['jpd113', 'jpd123'].map((code) => readJson(`server/JPLearn.Infrastructure/Data/Imports/exam/${code}.questions.json`)),
  );
  const courses = [];
  const allQuestions = [];

  for (const importFile of imports.sort((a, b) => courseOrder(a.courseCode) - courseOrder(b.courseCode))) {
    const courseCode = importFile.courseCode;
    const passages = (importFile.passages || []).map((seed) => ({
      id: `exam-${courseCode}-passage-${seed.id}`,
      courseCode,
      title: seed.title,
      content: seed.content,
      level: seed.level || importFile.level || 'N5',
      topic: normalizeTopic(seed.topic),
    }));
    const passageByOriginalId = new Map((importFile.passages || []).map((seed) => [seed.id, `exam-${courseCode}-passage-${seed.id}`]));

    const questions = (importFile.questions || []).map((seed) => {
      const questionId = `exam-${courseCode}-question-${seed.id}`;
      const options = (seed.options || []).map((option, index) => ({
        id: `${questionId}-option-${option.label || String.fromCharCode(65 + index)}`,
        label: option.label || String.fromCharCode(65 + index),
        text: option.text,
        isCorrect: Boolean(option.isCorrect),
      }));
      const correctOption = options.find((option) => option.isCorrect);
      return {
        id: questionId,
        courseCode,
        questionType: normalizeQuestionType(seed.type),
        topic: normalizeTopic(seed.topic),
        level: seed.level || importFile.level || 'N5',
        questionText: seed.questionText,
        passageId: seed.passageId ? passageByOriginalId.get(seed.passageId) : null,
        passage: seed.passageId ? passages.find((passage) => passage.id === passageByOriginalId.get(seed.passageId)) ?? null : null,
        options: options.map(({ isCorrect, ...option }) => option),
        correctOptionId: correctOption?.id || '',
        explanation: seed.explanation || '',
        orderIndex: seed.orderIndex ?? seed.id,
      };
    });

    const topics = (importFile.topics || []).map((topic) => {
      const code = normalizeTopic(topic.code);
      return {
        topic: code,
        label: topic.label,
        questionCount: questions.filter((question) => question.topic === code).length,
      };
    });
    const missingTopics = [...new Set(questions.map((question) => question.topic))]
      .filter((topic) => !topics.some((item) => item.topic === topic))
      .map((topic) => ({
        topic,
        label: topic.replaceAll('_', ' ').toUpperCase(),
        questionCount: questions.filter((question) => question.topic === topic).length,
      }));

    courses.push({
      id: `exam-${courseCode}`,
      code: courseCode,
      title: courseCode.toUpperCase(),
      description: `Ngân hàng câu hỏi luyện thi cho khóa học ${courseCode.toUpperCase()}`,
      accessTier: 'premium',
      packageCode: courseCode,
      isLocked: false,
      questionCount: questions.length,
      passageCount: passages.length,
    });

    allQuestions.push(...questions);
    await writeJson(`exam/${courseCode}/topics.json`, [...topics, ...missingTopics]);
    await writeJson(`exam/${courseCode}/questions.json`, questions.map(({ options, correctOptionId, explanation, passage, orderIndex, ...question }) => question));
    await writeJson(`exam/${courseCode}/question-details.json`, questions);
    await writeJson(`exam/${courseCode}/passages.json`, passages);
  }

  await writeJson('exam/courses.json', courses);
  await writeJson('exam/questions.json', allQuestions);
}

await rm(outRoot, { recursive: true, force: true });
await mkdir(outRoot, { recursive: true });
await generateVocabulary();
await generateSpeaking();
await generateKanji();
await generateGrammar();
await generateExam();
