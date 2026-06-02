import fs from 'fs';
import readline from 'readline';

const scanTranscript = async () => {
  const fileStream = fs.createReadStream('C:/Users/Admin/.gemini/antigravity-ide/brain/6b2cdba9-db84-49b5-948d-fb47d9a4922a/.system_generated/logs/transcript.jsonl');

  const rl = readline.createInterface({
    input: fileStream,
    crlfDelay: Infinity
  });

  let editCount = 0;
  for await (const line of rl) {
    if (line.includes('SpeakingCoursePage.tsx')) {
      const step = JSON.parse(line);
      console.log(`Step ${step.step_index}: Type: ${step.type}, Tool Calls: ${step.tool_calls ? step.tool_calls.map(tc => tc.name).join(', ') : 'none'}`);
      editCount++;
    }
  }
  console.log(`Total occurrences: ${editCount}`);
};

scanTranscript().catch(console.error);
