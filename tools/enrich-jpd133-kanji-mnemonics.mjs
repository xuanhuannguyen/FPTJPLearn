import { readFile, writeFile } from 'node:fs/promises';

const corePath = 'material/KANJI/jpd133_core.json';
const mnemonicPath = 'material/KANJI/jpd133_mnemonics.json';
const core = JSON.parse(await readFile(corePath, 'utf8'));
const mnemonics = JSON.parse(await readFile(mnemonicPath, 'utf8'));
let updated = 0;
for (const lesson of core) {
  for (const item of lesson.kanjiItems) {
    if (mnemonics[item.character]) {
      item.mnemonic = mnemonics[item.character];
      updated += 1;
    }
  }
}
if (updated !== 40) throw new Error(`Expected 40 JPD133 mnemonic updates, got ${updated}`);
await writeFile(corePath, `${JSON.stringify(core, null, 2)}\n`, 'utf8');
console.log(`Updated ${updated} JPD133 Kanji mnemonics.`);
