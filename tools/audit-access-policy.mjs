import fs from 'node:fs';

const checks = [
  {
    module: 'vocabulary',
    course: 'jpd113',
    path: 'server/JPLearn.Infrastructure/Data/Imports/vocabulary/jpd113.lessons.json',
    freeParentLessons: new Set([1]),
    parentLesson: (lesson) => Number.parseInt(String(lesson.title).split('-')[0], 10),
  },
  {
    module: 'vocabulary',
    course: 'jpd123',
    path: 'server/JPLearn.Infrastructure/Data/Imports/vocabulary/jpd123.lessons.json',
    freeParentLessons: new Set([4]),
    parentLesson: (lesson) => Number.parseInt(String(lesson.title).split('-')[0], 10),
  },
  {
    module: 'grammar',
    course: 'jpd113',
    path: 'server/JPLearn.Infrastructure/Data/Imports/grammar_jpd113.json',
    freeParentLessons: new Set([1]),
    parentLesson: (lesson) => Number(lesson.lessonNumber),
  },
  {
    module: 'grammar',
    course: 'jpd123',
    path: 'server/JPLearn.Infrastructure/Data/Imports/grammar_jpd123.json',
    freeParentLessons: new Set([4]),
    parentLesson: (lesson) => Number(lesson.lessonNumber),
  },
  {
    module: 'speaking',
    course: 'jpd113',
    path: 'server/JPLearn.Infrastructure/Data/Imports/speaking/jpd113.lessons.json',
    freeParentLessons: new Set([]),
    parentLesson: (lesson) => Number(lesson.id),
  },
  {
    module: 'speaking',
    course: 'jpd123',
    path: 'server/JPLearn.Infrastructure/Data/Imports/speaking/jpd123.lessons.json',
    freeParentLessons: new Set([]),
    parentLesson: (lesson) => Number(lesson.id),
  },
];

const failures = [];

for (const check of checks) {
  const data = JSON.parse(fs.readFileSync(check.path, 'utf8'));
  console.log(`\n== ${check.module} ${check.course} ==`);

  for (const lesson of data.lessons) {
    const parentLesson = check.parentLesson(lesson);
    const expected = check.freeParentLessons.has(parentLesson) ? 'free' : 'premium';
    const actual = String(lesson.accessTier || data.accessTier || 'free').trim().toLowerCase();
    const packageCode = lesson.packageCode || data.packageCode || data.courseCode || '';
    const ok = actual === expected;

    if (!ok) {
      failures.push(`${check.module} ${check.course} lesson ${lesson.lessonNumber ?? lesson.id}: expected ${expected}, got ${actual}`);
    }

    console.log(`${ok ? 'OK ' : 'BAD'} parent ${parentLesson}: ${actual} (${packageCode}) ${lesson.title || ''}`);
  }
}

console.log('\n== kanji ==');
const kanjiSeed = fs.readFileSync('server/JPLearn.Infrastructure/Data/Seed/KanjiSeedData.cs', 'utf8');
const expectedKanji = [
  { lesson: 1, course: 'jpd113', tier: 'free' },
  { lesson: 2, course: 'jpd113', tier: 'premium' },
  { lesson: 3, course: 'jpd113', tier: 'premium' },
  { lesson: 4, course: 'jpd123', tier: 'free' },
  { lesson: 5, course: 'jpd123', tier: 'premium' },
  { lesson: 6, course: 'jpd123', tier: 'premium' },
  { lesson: 7, course: 'jpd123', tier: 'premium' },
];

for (const item of expectedKanji) {
  const pattern = new RegExp(`LessonNumber = ${item.lesson}[^;]+AccessTier = "${item.tier}"[^;]+PackageCode = "kanji_${item.course}"`);
  const ok = pattern.test(kanjiSeed);
  if (!ok) {
    failures.push(`kanji ${item.course} lesson ${item.lesson}: expected ${item.tier}`);
  }
  console.log(`${ok ? 'OK ' : 'BAD'} lesson ${item.lesson}: ${item.tier} (kanji_${item.course})`);
}

console.log('\n== exam ==');
const examSeed = fs.readFileSync('server/JPLearn.Infrastructure/Data/Seed/ExamPracticeSeedData.cs', 'utf8');
const examOk = /AccessTier = "premium"/.test(examSeed) && /PackageCode = importFile\.CourseCode/.test(examSeed);
if (!examOk) {
  failures.push('exam practice courses should be premium and packageCode should map to course code');
}
console.log(`${examOk ? 'OK ' : 'BAD'} exam practice courses: premium`);

if (failures.length > 0) {
  console.log('\nFAILURES');
  for (const failure of failures) {
    console.log(`- ${failure}`);
  }
  process.exit(1);
}

console.log('\nAll access policies match expected launch rules.');
