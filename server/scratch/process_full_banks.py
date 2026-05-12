import json
import os
import re

def process_bank(input_file, course_code):
    if not os.path.exists(input_file):
        print(f"File not found: {input_file}")
        return None

    with open(input_file, 'r', encoding='utf-8') as f:
        data = json.load(f)

    print(f"Processing {course_code}: {len(data)} items found.")

    unique_questions = {}
    passages = []
    passage_id_counter = 1
    
    # Mapping table
    topic_mapping = {
        'kanji': ['hán tự'],
        'vocabulary': ['từ vựng', 'phân loại từ vựng'],
        'grammar': ['ngữ pháp', 'sắp xếp câu'],
        'conversation': ['giao tiếp'],
        'reading': ['đọc hiểu']
    }

    def map_topic(raw_type):
        if not raw_type:
            return 'vocabulary'
        rt = raw_type.lower()
        for target, keywords in topic_mapping.items():
            if any(k in rt for k in keywords):
                return target
        return 'vocabulary'

    current_passage = None
    current_passage_id = None

    for item in data:
        q_text = item.get('question', '').strip()
        if not q_text:
            continue
        
        raw_type = item.get('type', '')
        topic = map_topic(raw_type)
        is_reading = topic == 'reading'
        
        # Deduplication based on question text
        if q_text in unique_questions:
            continue
            
        # Detect Passage in Reading questions
        # Pattern 1: 「...」
        # Pattern 2: Đọc đoạn văn sau: ...
        # Pattern 3: Dựa trên đoạn văn sau: ...
        
        passage_text = ""
        actual_question = q_text
        
        if is_reading:
            # Try to extract passage between brackets
            bracket_match = re.search(r'「(.*?)」', q_text, re.DOTALL)
            if bracket_match:
                passage_text = bracket_match.group(1).strip()
                # Remove passage from question text if it's too long
                if len(passage_text) > 20:
                    actual_question = q_text.replace(f'「{passage_text}」', '').strip()
            else:
                # Try to extract after colon
                if ':' in q_text and len(q_text) > 100:
                    parts = q_text.split(':', 1)
                    if 'đoạn văn' in parts[0].lower():
                        passage_text = parts[1].strip()
                        actual_question = parts[0].strip()

        # Handle fragmented passages (e.g. "___①___ ... ___②___")
        # If the question has a snippet of a passage, we can use it as the passage.
        if is_reading and not passage_text:
            if '___' in q_text or 'đoạn văn' in q_text.lower():
                passage_text = q_text # Use the whole text as passage for now
        
        assigned_passage_id = None
        if passage_text:
            # Check if this passage already exists (to group questions)
            existing_passage = next((p for p in passages if p['content'] == passage_text or passage_text in p['content'] or p['content'] in passage_text), None)
            if existing_passage:
                assigned_passage_id = existing_passage['id']
                # Update existing passage content if this one is longer/better
                if len(passage_text) > len(existing_passage['content']):
                    existing_passage['content'] = passage_text
            else:
                assigned_passage_id = passage_id_counter
                passages.append({
                    'id': assigned_passage_id,
                    'title': f"Đoạn văn {passage_id_counter}",
                    'content': passage_text,
                    'topic': 'reading',
                    'level': 'N5'
                })
                passage_id_counter += 1

        # Structure for seeder
        options = []
        for opt in item.get('options', []):
            options.append({
                'label': opt.get('label', ''),
                'text': opt.get('text', ''),
                'isCorrect': opt.get('label', '') == item.get('answer', '')
            })
            
        if len(options) < 2 or not any(o['isCorrect'] for o in options):
            continue

        unique_questions[q_text] = {
            'id': len(unique_questions) + 1,
            'passageId': assigned_passage_id,
            'type': 'reading' if assigned_passage_id else 'standalone',
            'topic': topic,
            'level': 'N5',
            'questionText': actual_question if actual_question else q_text,
            'explanation': item.get('explanation', ''),
            'options': options,
            'orderIndex': len(unique_questions) + 1
        }

    # Final structure
    topics = [
        {'code': 'kanji', 'label': 'Hán tự', 'orderIndex': 1},
        {'code': 'vocabulary', 'label': 'Từ vựng', 'orderIndex': 2},
        {'code': 'grammar', 'label': 'Ngữ pháp', 'orderIndex': 3},
        {'code': 'conversation', 'label': 'Giao tiếp', 'orderIndex': 4},
        {'code': 'reading', 'label': 'Đọc hiểu', 'orderIndex': 5},
    ]

    result = {
        'courseCode': course_code,
        'level': 'N5',
        'topics': topics,
        'passages': passages,
        'questions': list(unique_questions.values())
    }

    print(f"Finished {course_code}: {len(result['questions'])} unique items, {len(passages)} passages.")
    return result

# Paths
jpd113_in = 'material/JPD113/JPD113_Full_Bank.json'
jpd123_in = 'material/JPD123/JPD123_FULLBANK.json'

output_dir = 'server/JPLearn.Infrastructure/Data/Imports/exam'
os.makedirs(output_dir, exist_ok=True)

# JPD113
res113 = process_bank(jpd113_in, 'jpd113')
if res113:
    with open(os.path.join(output_dir, 'jpd113.questions.json'), 'w', encoding='utf-8') as f:
        json.dump(res113, f, ensure_ascii=False, indent=2)

# JPD123
res123 = process_bank(jpd123_in, 'jpd123')
if res123:
    with open(os.path.join(output_dir, 'jpd123.questions.json'), 'w', encoding='utf-8') as f:
        json.dump(res123, f, ensure_ascii=False, indent=2)
