import os
import re

filepath = 'client/src/features/review/ReviewWorkspace.tsx'
with open(filepath, 'r', encoding='utf-8') as f:
    content = f.read()

# Add imports
if 'FlashcardMode' not in content:
    content = content.replace("from './types';", "from './types';\nimport { FlashcardMode } from './components/FlashcardMode';\nimport { MultipleChoiceMode } from './components/MultipleChoiceMode';\nimport { TypingMode } from './components/TypingMode';")

# Replace render block
start_marker = "            <div className=\"space-y-6\">\n              {renderPrompt(currentCard)}"
end_marker = "                </button>\n              )}\n            </div>"

start_idx = content.find(start_marker)
end_idx = content.find(end_marker) + len(end_marker)

if start_idx != -1 and end_idx != -1:
    new_render = """            <div className="w-full">
              {selectedMode === 'flashcard' && (
                <FlashcardMode
                  card={currentCard}
                  direction={selectedDirection}
                  onAnswer={(quality, correct, message) => finalizeAnswer(quality, correct, message)}
                  revealBack={session.revealBack}
                  onReveal={() => setSession({ ...session, revealBack: true })}
                  answered={session.answered}
                />
              )}

              {selectedMode === 'multichoice' && (
                <MultipleChoiceMode
                  card={currentCard}
                  options={options}
                  direction={selectedDirection}
                  onAnswer={(quality, correct, message) => finalizeAnswer(quality, correct, message)}
                  answered={session.answered}
                  feedback={session.feedback}
                  onNext={nextCard}
                />
              )}

              {selectedMode === 'typing' && (
                <TypingMode
                  card={currentCard}
                  direction={selectedDirection}
                  onAnswer={(quality, correct, message) => finalizeAnswer(quality, correct, message)}
                  answered={session.answered}
                  feedback={session.feedback}
                  onNext={nextCard}
                />
              )}
            </div>"""
    
    content = content[:start_idx] + new_render + content[end_idx:]

with open(filepath, 'w', encoding='utf-8') as f:
    f.write(content)
print("Done")
