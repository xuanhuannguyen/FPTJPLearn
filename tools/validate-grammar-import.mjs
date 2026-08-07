import { readFile } from 'node:fs/promises';

const filePath = process.argv[2] ?? 'server/JPLearn.Infrastructure/Data/Imports/grammar_jpd133.json';
const data = JSON.parse(await readFile(filePath, 'utf8'));
const errors = [];
const allowedExerciseTypes = new Set(['vi_to_ja', 'ja_to_vi', 'arrange']);
const seen = new Set();

if (data.courseCode !== 'jpd133' || data.level !== 'N5') errors.push('courseCode/level mismatch');
if (data.lessons?.length !== 4) errors.push('expected exactly four lessons');
for (const lesson of data.lessons ?? []) {
  if (seen.has(lesson.lessonNumber)) errors.push(`duplicate lesson ${lesson.lessonNumber}`);
  seen.add(lesson.lessonNumber);
  const patternOrders = new Set();
  for (const pattern of lesson.patterns ?? []) {
    if (!pattern.pattern || !pattern.title || !pattern.meaning || !pattern.structure) errors.push(`empty required pattern field in lesson ${lesson.lessonNumber}`);
    if (patternOrders.has(pattern.orderIndex)) errors.push(`duplicate pattern order in lesson ${lesson.lessonNumber}`);
    patternOrders.add(pattern.orderIndex);
    if ((pattern.examples ?? []).length < 2) errors.push(`fewer than two examples in ${pattern.title}`);
    const exampleOrders = new Set();
    for (const example of pattern.examples ?? []) {
      if (!example.japanese || !example.reading || !example.meaning) errors.push(`incomplete example in ${pattern.title}`);
      if (exampleOrders.has(example.orderIndex)) errors.push(`duplicate example order in ${pattern.title}`);
      exampleOrders.add(example.orderIndex);
    }
    const exerciseOrders = new Set();
    for (const exercise of pattern.exercises ?? []) {
      if (!allowedExerciseTypes.has(exercise.exerciseType)) errors.push(`invalid exercise type in ${pattern.title}`);
      if (!exercise.expectedAnswer) errors.push(`missing expected answer in ${pattern.title}`);
      if (exercise.exerciseType === 'arrange' && (!exercise.options?.length || !exercise.correctOrder?.length)) errors.push(`invalid arrange exercise in ${pattern.title}`);
      if (exerciseOrders.has(exercise.orderIndex)) errors.push(`duplicate exercise order in ${pattern.title}`);
      exerciseOrders.add(exercise.orderIndex);
      if (/\[CẦN BỔ SUNG\]|\*\*|\| Trường|\[ \]/u.test(JSON.stringify(exercise))) errors.push(`unprocessed markdown in ${pattern.title}`);
    }
  }
}
if (errors.length) {
  console.error(errors.join('\n'));
  process.exitCode = 1;
} else {
  console.log(`Valid: ${data.lessons.length} lessons, ${data.lessons.flatMap((lesson) => lesson.patterns).length} patterns.`);
}
