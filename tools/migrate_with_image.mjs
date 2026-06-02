import { readFile, writeFile, mkdir } from 'node:fs/promises';
import { copyFileSync } from 'node:fs';
import path from 'node:path';

const migrateWithImage = async () => {
  const mdPath = 'b:/FPT/Project/FPT_JPD/material/Luyện Nói/Vấn đáp JPTJPLearn/JPD113/Bài 1/CoTranh.md';
  const outDir = 'b:/FPT/Project/FPT_JPD/client/public/data/speaking/jpd113/qa';
  const outPath = path.join(outDir, 'lesson1_with_image.json');

  console.log('Creating output directory...');
  await mkdir(outDir, { recursive: true });

  console.log('Copying images...');
  copyFileSync(
    'b:/FPT/Project/FPT_JPD/material/Luyện Nói/Vấn đáp JPTJPLearn/JPD113/Bài 1/nhanvat1.png',
    path.join(outDir, 'nhanvat1.png')
  );
  copyFileSync(
    'b:/FPT/Project/FPT_JPD/material/Luyện Nói/Vấn đáp JPTJPLearn/JPD113/Bài 1/nhanvat2.png',
    path.join(outDir, 'nhanvat2.png')
  );
  console.log('Images copied successfully.');

  console.log('Reading CoTranh.md...');
  const rawContent = await readFile(mdPath, 'utf8');
  
  // Find the first '{' to parse it as JSON
  const firstBraceIndex = rawContent.indexOf('{');
  if (firstBraceIndex === -1) {
    throw new Error('Could not find starting JSON brace in CoTranh.md');
  }
  const jsonContent = rawContent.slice(firstBraceIndex);
  
  console.log('Parsing JSON...');
  const data = JSON.parse(jsonContent);

  console.log('Processing picture sets...');
  const processedPictureSets = [];
  
  for (const set of data.pictureSets || []) {
    const isAn = set.pictureId.includes('an');
    const imageUrl = `/data/speaking/jpd113/qa/${isAn ? 'nhanvat1.png' : 'nhanvat2.png'}`;
    
    const processedQuestions = [];
    let order = 1;
    
    for (const q of set.questions || []) {
      // Flatten relatedVocabulary
      const vocabItems = [];
      if (q.relatedVocabulary && Array.isArray(q.relatedVocabulary.items)) {
        for (const item of q.relatedVocabulary.items) {
          vocabItems.push({
            word: item.word,
            reading: item.reading,
            meaning: item.meaning
          });
        }
      }
      
      processedQuestions.push({
        questionId: q.id,
        order: order++,
        question: {
          ja: q.question.ja,
          vi: q.question.vi
        },
        answerType: 'text',
        sampleAnswers: q.sampleAnswers || [],
        relatedVocabulary: vocabItems,
        tips: q.tips || [],
        explanation: q.explanation || '',
        relatedGrammar: q.relatedGrammar || []
      });
    }
    
    processedPictureSets.push({
      pictureId: set.pictureId,
      pictureTitle: set.pictureTitle,
      imageUrl,
      questions: processedQuestions
    });
  }

  // Build the final output JSON
  const outputData = {
    courseCode: data.courseCode.toLowerCase(),
    lessonNumber: data.lessonNumber,
    lessonTitle: data.lessonTitle,
    questionMode: 'WITH_IMAGE',
    dataPurpose: 'oral_exam_practice_web',
    pictureSets: processedPictureSets
  };

  console.log('Writing output JSON...');
  await writeFile(outPath, JSON.stringify(outputData, null, 2), 'utf8');
  console.log('Migration completed successfully!');
};

migrateWithImage().catch(err => {
  console.error('Migration failed:', err);
  process.exit(1);
});
