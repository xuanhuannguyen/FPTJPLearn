import json
import os

def check_json_file(file_path):
    print(f"\nChecking: {file_path}")
    with open(file_path, 'r', encoding='utf-8') as f:
        data = json.load(f)
    
    questions = data.get('questions', [])
    if not questions:
        # Check if it's a lesson format (vocabulary/speaking/grammar)
        lessons = data.get('lessons', [])
        if lessons:
            for lesson in lessons:
                check_items(lesson.get('items', []), file_path, f"Lesson: {lesson.get('title')}")
        return

    check_items(questions, file_path, "Exam Questions")

def check_items(items, file_path, context):
    duplicates = 0
    no_correct = 0
    same_options = 0
    
    seen_texts = {}

    for item in items:
        item_id = item.get('id') or item.get('orderIndex')
        # Check duplicate question text
        text = item.get('questionText') or item.get('kanji') or item.get('hiragana') or item.get('text')
        if text:
            if text in seen_texts:
                duplicates += 1
                print(f"  [Error] Duplicate text in Item ID {item_id}: '{text[:50]}...' (Same as ID {seen_texts[text]})")
            else:
                seen_texts[text] = item_id

        # Check options
        options = item.get('options', [])
        if options:
            texts = [opt.get('text') for opt in options]
            if len(texts) != len(set(texts)):
                same_options += 1
                print(f"  [Error] Duplicate options in Item ID {item_id}: {texts}")
            
            correct_count = sum(1 for opt in options if opt.get('isCorrect') == True)
            if correct_count == 0:
                no_correct += 1
                print(f"  [Error] No correct answer in Item ID {item_id}")
            elif correct_count > 1:
                pass # Some might have multiple, but usually not

    print(f"  - Context: {context}")
    print(f"  - Duplicate items: {duplicates}")
    print(f"  - Items with duplicate options: {same_options}")
    print(f"  - Items with no correct answer: {no_correct}")

files_to_check = [
    r"b:\FPT\Project\FPT_JPD\server\JPLearn.Infrastructure\Data\Imports\exam\jpd113.questions.json",
    r"b:\FPT\Project\FPT_JPD\server\JPLearn.Infrastructure\Data\Imports\exam\jpd123.questions.json",
    r"b:\FPT\Project\FPT_JPD\server\JPLearn.Infrastructure\Data\Imports\vocabulary\jpd113.lessons.json",
    r"b:\FPT\Project\FPT_JPD\server\JPLearn.Infrastructure\Data\Imports\vocabulary\jpd123.lessons.json",
    r"b:\FPT\Project\FPT_JPD\server\JPLearn.Infrastructure\Data\Imports\grammar_jpd113.json",
    r"b:\FPT\Project\FPT_JPD\server\JPLearn.Infrastructure\Data\Imports\grammar_jpd123.json"
]

for f in files_to_check:
    if os.path.exists(f):
        check_json_file(f)
    else:
        print(f"File not found: {f}")
