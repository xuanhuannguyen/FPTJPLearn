import os
import re

filepath = 'client/src/features/review/ReviewWorkspace.tsx'
with open(filepath, 'r', encoding='utf-8') as f:
    content = f.read()

# Remove typingValue
content = re.sub(r'const \[typingValue, setTypingValue\] = useState\(\'\'\);\n\s*', '', content)
content = re.sub(r'setTypingValue\(\'\'\);\n\s*', '', content)

# Remove normalized
content = re.sub(r'const normalized = \(value: string\) => value\.trim\(\)\.toLowerCase\(\);\n\s*', '', content)

# Remove renderPrompt
content = re.sub(r'const renderPrompt = \(card: ReviewCard\) => \{.*?\};\n\s*', '', content, flags=re.DOTALL)

# Remove renderFlashcardAnswer
content = re.sub(r'const renderFlashcardAnswer = \(card: ReviewCard\) => \{.*?\};\n\s*', '', content, flags=re.DOTALL)

with open(filepath, 'w', encoding='utf-8') as f:
    f.write(content)
print("Done")
