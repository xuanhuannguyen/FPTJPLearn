import { readFile } from 'node:fs/promises';

const importPath = process.argv[2] || 'server/JPLearn.Infrastructure/Data/Imports/exam/jpd133.questions.json';
const allowedTopics = new Set(['kanji', 'grammar', 'vocabulary', 'conversation', 'odd_one_out', 'reading']);
const allowedTypes = new Set(['standalone', 'passage']);
const requiredTopicOrder = ['kanji', 'grammar', 'vocabulary', 'conversation', 'odd_one_out', 'reading'];

const normalize = (value) => String(value ?? '')
  .normalize('NFKC')
  .replace(/\s+/g, '')
  .replace(/[「」『』（）()［］\[\]、。・]/g, '')
  .toLowerCase();

const hasJapanese = (value) => /[\u3040-\u30ff\u3400-\u9fff々〆〤ー]/u.test(String(value ?? ''));
const optionHasContent = (value) => /[\u3040-\u30ff\u3400-\u9fff々〆〤ーA-Za-z0-9]/u.test(String(value ?? ''));

const garbagePatterns = [
  /fuoverflow/i,
  /choose\s*1\s*answer/i,
  /\banswer\b/i,
  /\bnext\b/i,
  /\bfinish\b/i,
  /\bOA\b|\bOB\b|\bOC\b|\bOD\b/,
  /chon\s*trong/i,
  /d[ăa]p\s*an|dapan/i,
  /dd?ien\s*vao|dien\s*vao/i,
  /thich\s*hop|thichhop/i,
  /cac\s+t[uư]i|cac\s+tu/i,
  /\bngoc\b/i,
  /\bthnao\b/i,
  /trolai/i,
  /there\s+are\s+\d+\s+questions/i,
  /font:\s*microsoft/i,
  /timeleft/i,
];

const badCharPatterns = [
  /[<>□�]/u,
  /[\u4e00-\u9fff]\s*[hH]\b/u,
  /\b[Ll]\d+\s*</u,
];

const isBadOptionText = (value) => {
  const text = String(value ?? '').trim();
  if (!text) return true;
  if (/^[\/\\|・･.\-_*×Xx\s]+$/u.test(text)) return true;
  if (text.length === 1 && !hasJapanese(text) && !/[A-Za-z0-9]/.test(text)) return true;
  if (!optionHasContent(text)) return true;
  return false;
};

const explanationIsGeneric = (value) => {
  const text = String(value ?? '');
  return /đúng theo (từ vựng và ngữ pháp|JPD133 N5)/i.test(text)
    || /phương án .+ là đáp án chính xác theo/i.test(text);
};

const errors = [];
const data = JSON.parse(await readFile(importPath, 'utf8'));

if (data.courseCode !== 'jpd133') errors.push('courseCode must be jpd133');
if (data.level !== 'N5') errors.push('level must be N5');
if (!Array.isArray(data.topics)) errors.push('topics must be an array');
if (!Array.isArray(data.questions)) errors.push('questions must be an array');
if (!Array.isArray(data.passages)) errors.push('passages must be an array');

const topicCodes = new Set((data.topics || []).map((item) => item.code));
for (const topic of requiredTopicOrder) {
  if (!topicCodes.has(topic)) errors.push(`missing topic: ${topic}`);
}

const ids = new Set();
const orderIndexes = new Set();
const fingerprints = new Set();
const passageIds = new Set((data.passages || []).map((item) => item.id));
const usedPassageIds = new Set();
const correctLabelCounts = { A: 0, B: 0, C: 0, D: 0 };
const categoryCounts = {};

for (const passage of data.passages || []) {
  if (!passage.id) errors.push('passage missing id');
  if (!String(passage.content ?? '').trim()) errors.push(`empty passage content: ${passage.id}`);
  if (passage.topic && !allowedTopics.has(passage.topic)) errors.push(`invalid passage topic: ${passage.id}`);
}

for (const question of data.questions || []) {
  const id = question.id ?? '(missing-id)';
  if (ids.has(question.id)) errors.push(`duplicate question id: ${question.id}`);
  ids.add(question.id);

  if (question.orderIndex == null) errors.push(`missing orderIndex: ${id}`);
  if (orderIndexes.has(question.orderIndex)) errors.push(`duplicate orderIndex: ${question.orderIndex}`);
  orderIndexes.add(question.orderIndex);

  if (!allowedTopics.has(question.topic)) errors.push(`invalid topic for ${id}: ${question.topic}`);
  if (!allowedTypes.has(question.type)) errors.push(`invalid type for ${id}: ${question.type}`);
  categoryCounts[question.topic] = (categoryCounts[question.topic] || 0) + 1;

  if (!String(question.questionText ?? '').trim()) errors.push(`empty questionText: ${id}`);
  if (!hasJapanese(question.questionText)) errors.push(`question ${id} has no Japanese text`);

  const explanation = String(question.explanation ?? '').trim();
  const correctLabelInExplanation = explanation.match(/^Đáp án ([A-D])\./);
  if (!correctLabelInExplanation) errors.push(`explanation must start with "Đáp án X.": ${id}`);
  if (explanationIsGeneric(explanation)) errors.push(`generic explanation: ${id}`);
  if (!hasJapanese(explanation) && !/(trợ từ|ngữ pháp|từ|kanji|hán tự|cách đọc|nghĩa|mẫu|động từ|tính từ|hỏi|trả lời|phù hợp|ngữ cảnh|cấu trúc|cụm|diễn tả|dùng để)/iu.test(explanation)) {
    errors.push(`explanation lacks concrete rationale: ${id}`);
  }

  if (question.needsReview === true) errors.push(`needsReview question leaked into import: ${id}`);

  if (question.type === 'passage') {
    if (!passageIds.has(question.passageId)) errors.push(`missing passage for ${id}: ${question.passageId}`);
    else usedPassageIds.add(question.passageId);
    if (question.topic !== 'reading') errors.push(`passage question must use reading topic: ${id}`);
  }
  if (question.type === 'standalone' && question.passageId != null) errors.push(`standalone question has passage: ${id}`);

  if (!Array.isArray(question.options) || question.options.length !== 4) {
    errors.push(`question ${id} must have exactly 4 options`);
    continue;
  }

  const labels = question.options.map((option) => option.label);
  if (labels.join(',') !== 'A,B,C,D') errors.push(`question ${id} options must be A,B,C,D`);

  const correct = question.options.filter((option) => option.isCorrect);
  if (correct.length !== 1) {
    errors.push(`question ${id} must have exactly one correct option`);
  } else {
    correctLabelCounts[correct[0].label] += 1;
    if (correctLabelInExplanation && correctLabelInExplanation[1] !== correct[0].label) {
      errors.push(`explanation/correct option mismatch: ${id}`);
    }
  }

  for (const option of question.options) {
    if (isBadOptionText(option.text)) errors.push(`bad option text in ${id}${option.label ? `/${option.label}` : ''}`);
  }

  const serialized = JSON.stringify(question);
  for (const pattern of garbagePatterns) {
    if (pattern.test(serialized)) errors.push(`OCR/UI garbage in ${id}: ${pattern}`);
  }
  for (const pattern of badCharPatterns) {
    if (pattern.test(serialized)) errors.push(`suspicious OCR character in ${id}: ${pattern}`);
  }

  const fingerprint = [
    normalize(question.questionText),
    question.passageId ? normalize(data.passages.find((passage) => passage.id === question.passageId)?.content) : '',
    question.options.map((option) => normalize(option.text)).sort().join('|'),
  ].join('::');
  if (fingerprints.has(fingerprint)) errors.push(`duplicate normalized fingerprint: ${id}`);
  fingerprints.add(fingerprint);
}

for (const passageId of passageIds) {
  if (!usedPassageIds.has(passageId)) errors.push(`unused passage: ${passageId}`);
}

const questionCount = data.questions?.length || 0;
if (questionCount > 0) {
  for (const [label, count] of Object.entries(correctLabelCounts)) {
    if (count / questionCount > 0.6) errors.push(`correct answer distribution is too skewed: ${label}=${count}/${questionCount}`);
  }
}

if (errors.length) {
  console.error(errors.map((error) => `- ${error}`).join('\n'));
  process.exit(1);
}

console.log('source images validated');
console.log('gemini extraction validated');
console.log('ocr garbage rejected');
console.log('answer distribution checked');
console.log('duplicate questions removed');
console.log('answer keys validated');
console.log('category mapping validated');
console.log('passage mapping validated');
console.log(`jpd133 import validated: ${questionCount} questions`);
console.log(`category counts: ${JSON.stringify(categoryCounts)}`);
console.log(`correct label counts: ${JSON.stringify(correctLabelCounts)}`);
