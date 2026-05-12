import json
import os
import re

def update_jpd113():
    material_path = r'b:\FPT\Project\FPT_JPD\material\JPD113\JPD113_Full_Bank.json'
    import_path = r'b:\FPT\Project\FPT_JPD\server\JPLearn.Infrastructure\Data\Imports\exam\jpd113.questions.json'
    
    with open(material_path, 'r', encoding='utf-8') as f:
        material_data = json.load(f)
        
    with open(import_path, 'r', encoding='utf-8') as f:
        import_data = json.load(f)
        
    # Find questions with embedded passage in material
    # Structure in material: "question": "【Đoạn văn】\n...\n\nQuestion Text"
    passages = []
    passage_map = {} # content -> passage_id
    
    # Reset passages in import_data
    import_data['passages'] = []
    
    for m_q in material_data:
        if '【Đoạn văn】' in m_q.get('question', ''):
            parts = m_q['question'].split('\n\n')
            if len(parts) >= 2:
                passage_content = parts[0].replace('【Đoạn văn】\n', '').strip()
                question_text = parts[1].strip()
                
                if passage_content not in passage_map:
                    p_id = len(passages) + 1
                    passage_map[passage_content] = p_id
                    passages.append({
                        "id": p_id,
                        "title": f"Đoạn văn {p_id}",
                        "content": passage_content,
                        "topic": "reading",
                        "level": "N5"
                    })
                
                p_id = passage_map[passage_content]
                
                # Find matching question in import_data
                for i_q in import_data['questions']:
                    # Match by question text similarity or ID if they match
                    # In JPD113, IDs seem to match between material and import
                    if i_q['id'] == m_q['id']:
                        i_q['passageId'] = p_id
                        i_q['type'] = "reading"
                        i_q['topic'] = "reading"
                        # Update question text to exclude passage
                        i_q['questionText'] = question_text
                        break
                        
    import_data['passages'] = passages
    
    with open(import_path, 'w', encoding='utf-8') as f:
        json.dump(import_data, f, ensure_ascii=False, indent=2)
    print(f"Updated JPD113: {len(passages)} passages found.")

def update_jpd123():
    material_path = r'b:\FPT\Project\FPT_JPD\material\JPD123\jpd123_questions_final.json'
    import_path = r'b:\FPT\Project\FPT_JPD\server\JPLearn.Infrastructure\Data\Imports\exam\jpd123.questions.json'
    
    with open(material_path, 'r', encoding='utf-8') as f:
        material_data = json.load(f)
        
    with open(import_path, 'r', encoding='utf-8') as f:
        import_data = json.load(f)
        
    passages = []
    passage_map = {} # content -> passage_id
    
    # Reset passages in import_data
    import_data['passages'] = []
    
    for m_q in material_data:
        passage_content = m_q.get('passage')
        if passage_content:
            if passage_content not in passage_map:
                p_id = len(passages) + 1
                passage_map[passage_content] = p_id
                passages.append({
                    "id": p_id,
                    "title": f"Đoạn văn {p_id}",
                    "content": passage_content,
                    "topic": "reading",
                    "level": "N5"
                })
            
            p_id = passage_map[passage_content]
            
            # Find matching question in import_data
            for i_q in import_data['questions']:
                if i_q['id'] == m_q['id']:
                    i_q['passageId'] = p_id
                    i_q['type'] = "reading"
                    i_q['topic'] = "reading"
                    # In JPD123, material already has clean questionText
                    i_q['questionText'] = m_q['question']
                    break
                    
    import_data['passages'] = passages
    
    with open(import_path, 'w', encoding='utf-8') as f:
        json.dump(import_data, f, ensure_ascii=False, indent=2)
    print(f"Updated JPD123: {len(passages)} passages found.")

if __name__ == "__main__":
    update_jpd113()
    update_jpd123()
