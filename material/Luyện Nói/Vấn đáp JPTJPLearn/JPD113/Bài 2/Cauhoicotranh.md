```json
{
  "courseCode": "JPD113",
  "lessonNumber": 2,
  "lessonTitle": "Bài 2 - Câu hỏi có tranh",
  "questionMode": "IMAGE_BASED",
  "dataPurpose": "oral_exam_practice_web",
  "pictureSets": [
    {
      "pictureId": "jpd113_l2_picture_01_shopping_building",
      "pictureTitle": "Tranh 1 - ニコニコショッピングビル",
      "pictureContentForGeneration": {
        "purpose": "Tạo tranh tòa nhà mua sắm nhiều tầng để luyện hỏi đáp về vị trí, tầng và đồ vật trong Bài 2.",
        "sceneDescription": {
          "style": "Minh họa đơn giản dạng sơ đồ tầng, rõ ràng, phù hợp web học tiếng Nhật sơ cấp.",
          "buildingName": "ニコニコショッピングビル",
          "layout": "Tòa nhà có các tầng từ 地下1階 đến 5階. Mỗi tầng có biểu tượng đồ vật/địa điểm để học sinh nhìn và trả lời câu hỏi.",
          "visualRequirement": "Mỗi tầng cần có nhãn tầng rõ ràng bên trái và hình minh họa đồ vật/địa điểm bên phải."
        },
        "floorInformation": [
          {
            "floor": "5階",
            "items": ["レストラン", "食べ物", "トイレ"],
            "vi": "Tầng 5 có nhà hàng, đồ ăn và nhà vệ sinh."
          },
          {
            "floor": "4階",
            "items": ["カメラ", "でんしじしょ", "パソコン", "けいたいでんわ"],
            "vi": "Tầng 4 có máy ảnh, từ điển điện tử, máy tính và điện thoại."
          },
          {
            "floor": "3階",
            "items": ["１００円ショップ", "ペン", "けしゴム", "皿", "きつえんじょ", "トイレ"],
            "vi": "Tầng 3 có cửa hàng 100 yên, bút, tẩy, đĩa, khu hút thuốc và nhà vệ sinh."
          },
          {
            "floor": "2階",
            "items": ["くつ", "ズボン", "Tシャツ", "本"],
            "vi": "Tầng 2 có giày, quần, áo thun và sách."
          },
          {
            "floor": "1階",
            "items": ["きっさてん", "ケーキ", "ATM", "トイレ"],
            "vi": "Tầng 1 có quán cà phê, bánh kem, ATM và nhà vệ sinh."
          },
          {
            "floor": "地下1階",
            "items": ["スーパー", "魚", "パン", "りんご", "バナナ", "水"],
            "vi": "Tầng hầm 1 có siêu thị và thực phẩm."
          }
        ],
        "japaneseInfoForAnswering": {
          "restaurant": "ごかい",
          "supermarket": "ちか　いっかい",
          "pants": "にかい",
          "camera": "よんかい"
        }
      },
      "questions": [
        {
          "id": "l2_img_building_q01",
          "order": 1,
          "question": {
            "ja": "レストランは　ちか　いっかいですか。",
            "vi": "Nhà hàng ở tầng hầm 1 phải không?"
          },
          "sampleAnswers": [
            {
              "ja": "いいえ、ごかいです。",
              "vi": "Không, ở tầng 5."
            }
          ],
          "relatedGrammar": [
            {
              "pattern": "N は số tầng ですか。",
              "meaning": "N có ở tầng ... không?",
              "howToUse": "Dùng để xác nhận vị trí tầng của địa điểm trong tranh."
            },
            {
              "pattern": "いいえ、Nです。",
              "meaning": "Không, là N.",
              "howToUse": "Dùng khi thông tin trong câu hỏi sai và cần nói thông tin đúng."
            }
          ],
          "relatedVocabulary": {
            "topic": "Tầng và địa điểm trong tòa nhà",
            "items": [
              {
                "word": "レストラン",
                "reading": "レストラン",
                "meaning": "Nhà hàng"
              },
              {
                "word": "スーパー",
                "reading": "スーパー",
                "meaning": "Siêu thị"
              },
              {
                "word": "トイレ",
                "reading": "トイレ",
                "meaning": "Nhà vệ sinh"
              },
              {
                "word": "きっさてん",
                "reading": "きっさてん",
                "meaning": "Quán cà phê"
              },
              {
                "word": "ATM",
                "reading": "ATM",
                "meaning": "Máy ATM"
              },
              {
                "word": "ちか　いっかい",
                "reading": "ちか　いっかい",
                "meaning": "Tầng hầm 1"
              },
              {
                "word": "いっかい",
                "reading": "いっかい",
                "meaning": "Tầng 1"
              },
              {
                "word": "にかい",
                "reading": "にかい",
                "meaning": "Tầng 2"
              },
              {
                "word": "さんかい",
                "reading": "さんかい",
                "meaning": "Tầng 3"
              },
              {
                "word": "よんかい",
                "reading": "よんかい",
                "meaning": "Tầng 4"
              },
              {
                "word": "ごかい",
                "reading": "ごかい",
                "meaning": "Tầng 5"
              }
            ]
          },
          "explanation": "Trong tranh, レストラン nằm ở 5階, không phải 地下1階. Vì vậy câu trả lời đúng là いいえ、ごかいです。",
          "tips": [
            "Nghe thấy レストラン thì tìm tầng có hình đồ ăn/nhà hàng.",
            "Nếu câu hỏi hỏi sai tầng, dùng いいえ + tầng đúng.",
            "ごかい là tầng 5."
          ],
          "commonMistakes": [
            "Nhầm レストラン với スーパー.",
            "Nhìn thấy đồ ăn nhưng không kiểm tra tầng.",
            "Quên いいえ khi thông tin trong câu hỏi sai."
          ]
        },
        {
          "id": "l2_img_building_q02",
          "order": 2,
          "question": {
            "ja": "スーパーは　どこですか。",
            "vi": "Siêu thị ở đâu?"
          },
          "sampleAnswers": [
            {
              "ja": "ちか　いっかいです。",
              "vi": "Ở tầng hầm 1."
            }
          ],
          "relatedGrammar": [
            {
              "pattern": "N は どこですか。",
              "meaning": "N ở đâu?",
              "howToUse": "Dùng để hỏi vị trí của địa điểm hoặc đồ vật trong tranh."
            }
          ],
          "relatedVocabulary": {
            "topic": "Địa điểm và tầng",
            "items": [
              {
                "word": "スーパー",
                "reading": "スーパー",
                "meaning": "Siêu thị"
              },
              {
                "word": "レストラン",
                "reading": "レストラン",
                "meaning": "Nhà hàng"
              },
              {
                "word": "１００円ショップ",
                "reading": "ひゃくえんショップ",
                "meaning": "Cửa hàng 100 yên"
              },
              {
                "word": "きっさてん",
                "reading": "きっさてん",
                "meaning": "Quán cà phê"
              },
              {
                "word": "どこ",
                "reading": "どこ",
                "meaning": "Ở đâu"
              },
              {
                "word": "ちか　いっかい",
                "reading": "ちか　いっかい",
                "meaning": "Tầng hầm 1"
              },
              {
                "word": "いっかい",
                "reading": "いっかい",
                "meaning": "Tầng 1"
              },
              {
                "word": "ごかい",
                "reading": "ごかい",
                "meaning": "Tầng 5"
              }
            ]
          },
          "explanation": "Trong tranh, スーパー nằm ở 地下1階. Vì vậy trả lời ちか　いっかいです。",
          "tips": [
            "どこですか hỏi vị trí.",
            "Nếu vị trí là tầng, chỉ cần trả lời tầng + です。",
            "スーパー thường đi với hình thực phẩm như cá, bánh mì, trái cây, nước."
          ],
          "commonMistakes": [
            "Trả lời スーパーです, sai vì câu hỏi hỏi vị trí.",
            "Nhầm 地下1階 với 1階.",
            "Quên nói ちか khi trả lời tầng hầm."
          ]
        },
        {
          "id": "l2_img_building_q03",
          "order": 3,
          "question": {
            "ja": "ズボンは　どこですか。",
            "vi": "Quần ở đâu?"
          },
          "sampleAnswers": [
            {
              "ja": "にかいです。",
              "vi": "Ở tầng 2."
            }
          ],
          "relatedGrammar": [
            {
              "pattern": "N は どこですか。",
              "meaning": "N ở đâu?",
              "howToUse": "Dùng để hỏi vị trí đồ vật trong tranh."
            }
          ],
          "relatedVocabulary": {
            "topic": "Quần áo và đồ vật",
            "items": [
              {
                "word": "ズボン",
                "reading": "ズボン",
                "meaning": "Quần"
              },
              {
                "word": "Tシャツ",
                "reading": "Tシャツ",
                "meaning": "Áo thun"
              },
              {
                "word": "くつ",
                "reading": "くつ",
                "meaning": "Giày"
              },
              {
                "word": "本",
                "reading": "ほん",
                "meaning": "Sách"
              },
              {
                "word": "どこ",
                "reading": "どこ",
                "meaning": "Ở đâu"
              },
              {
                "word": "にかい",
                "reading": "にかい",
                "meaning": "Tầng 2"
              },
              {
                "word": "さんかい",
                "reading": "さんかい",
                "meaning": "Tầng 3"
              },
              {
                "word": "よんかい",
                "reading": "よんかい",
                "meaning": "Tầng 4"
              }
            ]
          },
          "explanation": "Trong tranh, ズボン nằm ở 2階 cùng với giày, áo thun và sách.",
          "tips": [
            "Nghe thấy ズボン thì tìm hình cái quần.",
            "Trả lời ngắn: にかいです。",
            "Có thể luyện thêm bằng cách thay ズボン thành くつ hoặc Tシャツ."
          ],
          "commonMistakes": [
            "Nhầm ズボン với Tシャツ.",
            "Trả lời bằng tên đồ vật thay vì tầng.",
            "Đọc 2階 sai thành にがい."
          ]
        },
        {
          "id": "l2_img_building_q04",
          "order": 4,
          "question": {
            "ja": "カメラは　なんかいですか。",
            "vi": "Máy ảnh ở tầng mấy?"
          },
          "sampleAnswers": [
            {
              "ja": "よんかいです。",
              "vi": "Ở tầng 4."
            }
          ],
          "relatedGrammar": [
            {
              "pattern": "N は なんかいですか。",
              "meaning": "N ở tầng mấy?",
              "howToUse": "Dùng để hỏi số tầng của đồ vật/địa điểm trong tranh."
            }
          ],
          "relatedVocabulary": {
            "topic": "Đồ điện tử và tầng",
            "items": [
              {
                "word": "カメラ",
                "reading": "カメラ",
                "meaning": "Máy ảnh"
              },
              {
                "word": "パソコン",
                "reading": "パソコン",
                "meaning": "Máy tính"
              },
              {
                "word": "けいたいでんわ",
                "reading": "けいたいでんわ",
                "meaning": "Điện thoại di động"
              },
              {
                "word": "でんしじしょ",
                "reading": "でんしじしょ",
                "meaning": "Từ điển điện tử"
              },
              {
                "word": "なんかい",
                "reading": "なんかい",
                "meaning": "Tầng mấy"
              },
              {
                "word": "よんかい",
                "reading": "よんかい",
                "meaning": "Tầng 4"
              }
            ]
          },
          "explanation": "Trong tranh, カメラ nằm ở 4階, cùng khu đồ điện tử.",
          "tips": [
            "なんかいですか bắt buộc trả lời bằng số tầng.",
            "4階 đọc là よんかい.",
            "Không đọc là しかい."
          ],
          "commonMistakes": [
            "Đọc 4階 sai.",
            "Nhầm カメラ với けいたいでんわ.",
            "Trả lời カメラです thay vì よんかいです."
          ]
        }
      ]
    },
    {
      "pictureId": "jpd113_l2_picture_02_bag_product",
      "pictureTitle": "Tranh 2 - ベトナムのかばん",
      "pictureContentForGeneration": {
        "purpose": "Tạo tranh thẻ sản phẩm túi/cặp để luyện hỏi đáp về tên đồ vật, giá tiền, sở hữu và xuất xứ.",
        "productCard": {
          "item": "かばん",
          "origin": "ベトナム",
          "price": "200,000 ドン",
          "owner": "マイさん"
        },
        "suggestedDisplayText": [
          "Product: かばん",
          "Origin: ベトナム",
          "Price: 200,000 ドン",
          "Owner: マイさん"
        ],
        "visualRequirement": "Tranh có hình một chiếc túi/cặp rõ ràng. Bên cạnh có thẻ thông tin: ベトナム, 200.000ドン, マイさん."
      },
      "questions": [
        {
          "id": "l2_img_bag_q01",
          "order": 1,
          "question": {
            "ja": "これは　なんですか。",
            "vi": "Đây là cái gì?"
          },
          "sampleAnswers": [
            {
              "ja": "かばんです。",
              "vi": "Là cái cặp/túi."
            }
          ],
          "relatedGrammar": [
            {
              "pattern": "これは N です。",
              "meaning": "Đây là N.",
              "howToUse": "Dùng để trả lời tên đồ vật trong tranh."
            }
          ],
          "relatedVocabulary": {
            "topic": "Tên đồ vật",
            "items": [
              {
                "word": "これ",
                "reading": "これ",
                "meaning": "Cái này"
              },
              {
                "word": "なん",
                "reading": "なん",
                "meaning": "Cái gì"
              },
              {
                "word": "かばん",
                "reading": "かばん",
                "meaning": "Cặp/túi"
              },
              {
                "word": "ズボン",
                "reading": "ズボン",
                "meaning": "Quần"
              },
              {
                "word": "Tシャツ",
                "reading": "Tシャツ",
                "meaning": "Áo thun"
              },
              {
                "word": "くつ",
                "reading": "くつ",
                "meaning": "Giày"
              },
              {
                "word": "とけい",
                "reading": "とけい",
                "meaning": "Đồng hồ"
              },
              {
                "word": "パソコン",
                "reading": "パソコン",
                "meaning": "Máy tính"
              }
            ]
          },
          "explanation": "Trong tranh là một chiếc túi/cặp, nên câu trả lời là かばんです。",
          "tips": [
            "これはなんですか hỏi tên đồ vật.",
            "Trả lời bằng tên đồ vật + です。",
            "Không cần trả lời giá hoặc người sở hữu nếu câu hỏi chỉ hỏi tên đồ vật."
          ],
          "commonMistakes": [
            "Trả lời ベトナムです dù đó là xuất xứ.",
            "Trả lời マイさんのです dù câu hỏi hỏi đồ vật.",
            "Quên です ở cuối câu."
          ]
        },
        {
          "id": "l2_img_bag_q02",
          "order": 2,
          "question": {
            "ja": "このかばんは　マイさんのですか。",
            "vi": "Cái túi/cặp này là của bạn Mai phải không?"
          },
          "sampleAnswers": [
            {
              "ja": "はい、マイさんのです。",
              "vi": "Vâng, là của bạn Mai."
            },
            {
              "ja": "はい、マイさんの　かばんです。",
              "vi": "Vâng, là cặp của bạn Mai."
            }
          ],
          "relatedGrammar": [
            {
              "pattern": "N は 人のですか。",
              "meaning": "N có phải là của người đó không?",
              "howToUse": "Dùng để xác nhận người sở hữu đồ vật."
            },
            {
              "pattern": "はい、人のです。",
              "meaning": "Vâng, là của người đó.",
              "howToUse": "Dùng khi thông tin sở hữu trong câu hỏi đúng."
            }
          ],
          "relatedVocabulary": {
            "topic": "Sở hữu đồ vật",
            "items": [
              {
                "word": "この",
                "reading": "この",
                "meaning": "Cái ... này"
              },
              {
                "word": "かばん",
                "reading": "かばん",
                "meaning": "Cặp/túi"
              },
              {
                "word": "マイさん",
                "reading": "マイさん",
                "meaning": "Bạn Mai"
              },
              {
                "word": "マイさんの",
                "reading": "マイさんの",
                "meaning": "Của bạn Mai"
              },
              {
                "word": "わたしの",
                "reading": "わたしの",
                "meaning": "Của tôi"
              },
              {
                "word": "Aさんの",
                "reading": "Aさんの",
                "meaning": "Của bạn A"
              },
              {
                "word": "だれの",
                "reading": "だれの",
                "meaning": "Của ai"
              },
              {
                "word": "の",
                "reading": "の",
                "meaning": "Của"
              }
            ]
          },
          "explanation": "Trong thẻ thông tin của tranh, owner là マイさん. Vì vậy trả lời はい、マイさんのです。",
          "tips": [
            "Câu có 人のですか hỏi xác nhận sở hữu.",
            "Nếu đúng người sở hữu, trả lời はい、マイさんのです。",
            "Không trả lời ベトナムのです vì ベトナム là xuất xứ."
          ],
          "commonMistakes": [
            "Nhầm マイさんの với ベトナムの.",
            "Quên の sau tên người.",
            "Trả lời giá tiền dù câu hỏi hỏi sở hữu."
          ]
        },
        {
          "id": "l2_img_bag_q03",
          "order": 3,
          "question": {
            "ja": "このかばんは　いくらですか。",
            "vi": "Cái túi/cặp này bao nhiêu tiền?"
          },
          "sampleAnswers": [
            {
              "ja": "にじゅうまんドンです。",
              "vi": "Là 200.000 đồng."
            },
            {
              "ja": "200,000ドンです。",
              "vi": "Là 200.000 đồng."
            }
          ],
          "relatedGrammar": [
            {
              "pattern": "N は いくらですか。",
              "meaning": "N bao nhiêu tiền?",
              "howToUse": "Dùng để hỏi giá tiền của sản phẩm trong tranh."
            }
          ],
          "relatedVocabulary": {
            "topic": "Giá tiền và đơn vị tiền",
            "items": [
              {
                "word": "いくら",
                "reading": "いくら",
                "meaning": "Bao nhiêu tiền"
              },
              {
                "word": "にじゅうまん",
                "reading": "にじゅうまん",
                "meaning": "200.000"
              },
              {
                "word": "ドン",
                "reading": "ドン",
                "meaning": "Đồng Việt Nam"
              },
              {
                "word": "えん",
                "reading": "えん",
                "meaning": "Yên Nhật"
              },
              {
                "word": "ドル",
                "reading": "ドル",
                "meaning": "Đô la"
              },
              {
                "word": "ごせん",
                "reading": "ごせん",
                "meaning": "5.000"
              },
              {
                "word": "ひゃく",
                "reading": "ひゃく",
                "meaning": "100"
              }
            ]
          },
          "explanation": "Trong tranh ghi giá 200.000ドン, tiếng Nhật đọc là にじゅうまんドン.",
          "tips": [
            "いくらですか hỏi giá tiền.",
            "Phải nói cả số tiền và đơn vị tiền.",
            "200.000 đọc là にじゅうまん."
          ],
          "commonMistakes": [
            "Quên ドン.",
            "Trả lời マイさんのです dù câu hỏi hỏi giá.",
            "Nhầm にじゅうまん với にまん."
          ]
        },
        {
          "id": "l2_img_bag_q04",
          "order": 4,
          "question": {
            "ja": "このかばんは　どこのですか。",
            "vi": "Cái túi/cặp này là của nước nào/hãng nào?"
          },
          "sampleAnswers": [
            {
              "ja": "ベトナムのです。",
              "vi": "Là của Việt Nam."
            },
            {
              "ja": "ベトナムの　かばんです。",
              "vi": "Là túi/cặp của Việt Nam."
            }
          ],
          "relatedGrammar": [
            {
              "pattern": "N は どこのですか。",
              "meaning": "N là của nước nào/hãng nào?",
              "howToUse": "Dùng để hỏi xuất xứ hoặc hãng của sản phẩm."
            }
          ],
          "relatedVocabulary": {
            "topic": "Xuất xứ sản phẩm",
            "items": [
              {
                "word": "どこの",
                "reading": "どこの",
                "meaning": "Của nước nào/hãng nào"
              },
              {
                "word": "ベトナム",
                "reading": "ベトナム",
                "meaning": "Việt Nam"
              },
              {
                "word": "日本",
                "reading": "にほん",
                "meaning": "Nhật Bản"
              },
              {
                "word": "アメリカ",
                "reading": "アメリカ",
                "meaning": "Mỹ"
              },
              {
                "word": "かんこく",
                "reading": "かんこく",
                "meaning": "Hàn Quốc"
              },
              {
                "word": "ちゅうごく",
                "reading": "ちゅうごく",
                "meaning": "Trung Quốc"
              },
              {
                "word": "の",
                "reading": "の",
                "meaning": "Của/thuộc về"
              }
            ]
          },
          "explanation": "Trong tranh ghi xuất xứ là ベトナム, nên trả lời ベトナムのです。",
          "tips": [
            "どこの hỏi xuất xứ hoặc hãng.",
            "Tên nước + のです là câu trả lời ngắn.",
            "Không thêm 人 sau tên nước vì đang nói đồ vật, không nói quốc tịch."
          ],
          "commonMistakes": [
            "Trả lời ベトナム人です, sai vì đang hỏi đồ vật.",
            "Quên の sau ベトナム.",
            "Nhầm どこの với だれの."
          ]
        },
        {
          "id": "l2_img_bag_q05",
          "order": 5,
          "question": {
            "ja": "このかばんは　アメリカのですか。",
            "vi": "Cái túi/cặp này là của Mỹ phải không?"
          },
          "sampleAnswers": [
            {
              "ja": "いいえ、ベトナムのです。",
              "vi": "Không, là của Việt Nam."
            },
            {
              "ja": "いいえ、アメリカのじゃありません。ベトナムのです。",
              "vi": "Không, không phải của Mỹ. Là của Việt Nam."
            }
          ],
          "relatedGrammar": [
            {
              "pattern": "N は 国のですか。",
              "meaning": "N có phải là của nước đó không?",
              "howToUse": "Dùng để xác nhận xuất xứ."
            },
            {
              "pattern": "いいえ、Nじゃありません。Nです。",
              "meaning": "Không, không phải N. Là N.",
              "howToUse": "Dùng khi thông tin trong câu hỏi sai."
            }
          ],
          "relatedVocabulary": {
            "topic": "Xác nhận xuất xứ",
            "items": [
              {
                "word": "アメリカ",
                "reading": "アメリカ",
                "meaning": "Mỹ"
              },
              {
                "word": "ベトナム",
                "reading": "ベトナム",
                "meaning": "Việt Nam"
              },
              {
                "word": "日本",
                "reading": "にほん",
                "meaning": "Nhật Bản"
              },
              {
                "word": "いいえ",
                "reading": "いいえ",
                "meaning": "Không"
              },
              {
                "word": "じゃありません",
                "reading": "じゃありません",
                "meaning": "Không phải"
              }
            ]
          },
          "explanation": "Trong tranh, túi/cặp là của ベトナム, không phải アメリカ. Vì vậy trả lời いいえ、ベトナムのです。",
          "tips": [
            "Nếu câu hỏi đưa thông tin sai, bắt đầu bằng いいえ.",
            "Có thể trả lời ngắn hoặc đầy đủ.",
            "Tên nước + の = của nước đó."
          ],
          "commonMistakes": [
            "Trả lời はい vì không kiểm tra thông tin trong tranh.",
            "Quên の sau tên nước.",
            "Thêm 人 sau tên nước khi nói đồ vật."
          ]
        }
      ]
    },
    {
      "pictureId": "jpd113_l2_picture_03_iphone_product",
      "pictureTitle": "Tranh 3 - アップルのアイフォン",
      "pictureContentForGeneration": {
        "purpose": "Tạo tranh thẻ sản phẩm điện thoại iPhone để luyện hỏi đáp về tên đồ vật, giá tiền, sở hữu và xuất xứ.",
        "productCard": {
          "item": "アイフォン／けいたいでんわ",
          "brand": "APPLE／アップル",
          "origin": "アメリカ",
          "price": "900 ドル",
          "owner": "アンさん"
        },
        "suggestedDisplayText": [
          "Product: iPhone / アイフォン",
          "Brand: APPLE（アップル）",
          "Origin: アメリカ",
          "Price: 900 ドル",
          "Owner: アンさん"
        ],
        "visualRequirement": "Tranh có hình điện thoại iPhone. Bên cạnh có thẻ thông tin: APPLE（アップル）, アメリカ, 900ドル, アンさん."
      },
      "questions": [
        {
          "id": "l2_img_iphone_q01",
          "order": 1,
          "question": {
            "ja": "これは　日本語で　なんですか。",
            "vi": "Cái này trong tiếng Nhật là gì?"
          },
          "sampleAnswers": [
            {
              "ja": "けいたいでんわです。",
              "vi": "Là điện thoại di động."
            },
            {
              "ja": "アイフォンです。",
              "vi": "Là iPhone."
            }
          ],
          "relatedGrammar": [
            {
              "pattern": "これは 日本語で なんですか。",
              "meaning": "Cái này trong tiếng Nhật là gì?",
              "howToUse": "Dùng để hỏi cách gọi đồ vật bằng tiếng Nhật."
            }
          ],
          "relatedVocabulary": {
            "topic": "Tên đồ điện tử",
            "items": [
              {
                "word": "日本語",
                "reading": "にほんご",
                "meaning": "Tiếng Nhật"
              },
              {
                "word": "けいたいでんわ",
                "reading": "けいたいでんわ",
                "meaning": "Điện thoại di động"
              },
              {
                "word": "アイフォン",
                "reading": "アイフォン",
                "meaning": "iPhone"
              },
              {
                "word": "パソコン",
                "reading": "パソコン",
                "meaning": "Máy tính"
              },
              {
                "word": "カメラ",
                "reading": "カメラ",
                "meaning": "Máy ảnh"
              },
              {
                "word": "でんしじしょ",
                "reading": "でんしじしょ",
                "meaning": "Từ điển điện tử"
              }
            ]
          },
          "explanation": "Trong tranh là điện thoại iPhone. Với bài sơ cấp, có thể trả lời けいたいでんわです hoặc アイフォンです。",
          "tips": [
            "Nghe 日本語で thì trả lời bằng tiếng Nhật.",
            "けいたいでんわ là cách nói chung: điện thoại di động.",
            "アイフォン là tên sản phẩm cụ thể."
          ],
          "commonMistakes": [
            "Trả lời アップルです dù đó là hãng.",
            "Trả lời アメリカです dù đó là xuất xứ.",
            "Quên です cuối câu."
          ]
        },
        {
          "id": "l2_img_iphone_q02",
          "order": 2,
          "question": {
            "ja": "このアイフォンは　いくらですか。",
            "vi": "Chiếc iPhone này bao nhiêu tiền?"
          },
          "sampleAnswers": [
            {
              "ja": "きゅうひゃくドルです。",
              "vi": "Là 900 đô la."
            },
            {
              "ja": "900ドルです。",
              "vi": "Là 900 đô la."
            }
          ],
          "relatedGrammar": [
            {
              "pattern": "N は いくらですか。",
              "meaning": "N bao nhiêu tiền?",
              "howToUse": "Dùng để hỏi giá tiền của sản phẩm."
            }
          ],
          "relatedVocabulary": {
            "topic": "Giá tiền điện thoại",
            "items": [
              {
                "word": "アイフォン",
                "reading": "アイフォン",
                "meaning": "iPhone"
              },
              {
                "word": "いくら",
                "reading": "いくら",
                "meaning": "Bao nhiêu tiền"
              },
              {
                "word": "きゅうひゃく",
                "reading": "きゅうひゃく",
                "meaning": "900"
              },
              {
                "word": "ドル",
                "reading": "ドル",
                "meaning": "Đô la"
              },
              {
                "word": "ドン",
                "reading": "ドン",
                "meaning": "Đồng Việt Nam"
              },
              {
                "word": "えん",
                "reading": "えん",
                "meaning": "Yên Nhật"
              }
            ]
          },
          "explanation": "Trong tranh ghi giá 900ドル, tiếng Nhật đọc là きゅうひゃくドル.",
          "tips": [
            "いくらですか hỏi giá tiền.",
            "Phải nói số tiền + đơn vị tiền.",
            "900 đọc là きゅうひゃく."
          ],
          "commonMistakes": [
            "Quên ドル.",
            "Trả lời アップルのです dù câu hỏi hỏi giá.",
            "Nhầm きゅうひゃく với きゅうじゅう."
          ]
        },
        {
          "id": "l2_img_iphone_q03",
          "order": 3,
          "question": {
            "ja": "このアイフォンは　アンさんのですか。",
            "vi": "Chiếc iPhone này là của bạn An phải không?"
          },
          "sampleAnswers": [
            {
              "ja": "はい、アンさんのです。",
              "vi": "Vâng, là của bạn An."
            },
            {
              "ja": "はい、アンさんの　アイフォンです。",
              "vi": "Vâng, là iPhone của bạn An."
            }
          ],
          "relatedGrammar": [
            {
              "pattern": "N は 人のですか。",
              "meaning": "N có phải là của người đó không?",
              "howToUse": "Dùng để xác nhận người sở hữu đồ vật."
            }
          ],
          "relatedVocabulary": {
            "topic": "Sở hữu điện thoại",
            "items": [
              {
                "word": "アンさん",
                "reading": "アンさん",
                "meaning": "Bạn An"
              },
              {
                "word": "アンさんの",
                "reading": "アンさんの",
                "meaning": "Của bạn An"
              },
              {
                "word": "わたしの",
                "reading": "わたしの",
                "meaning": "Của tôi"
              },
              {
                "word": "だれの",
                "reading": "だれの",
                "meaning": "Của ai"
              },
              {
                "word": "アイフォン",
                "reading": "アイフォン",
                "meaning": "iPhone"
              },
              {
                "word": "の",
                "reading": "の",
                "meaning": "Của"
              }
            ]
          },
          "explanation": "Trong tranh ghi owner là アンさん. Vì vậy trả lời はい、アンさんのです。",
          "tips": [
            "Nghe thấy 人のですか thì xác nhận sở hữu.",
            "Nếu đúng, trả lời はい、アンさんのです。",
            "Không trả lời アップルのです vì アップル là hãng."
          ],
          "commonMistakes": [
            "Nhầm アンさん với アップル.",
            "Quên の sau tên người.",
            "Trả lời giá tiền dù câu hỏi hỏi sở hữu."
          ]
        },
        {
          "id": "l2_img_iphone_q04",
          "order": 4,
          "question": {
            "ja": "このアイフォンは　どこのですか。",
            "vi": "Chiếc iPhone này là của hãng/nước nào?"
          },
          "sampleAnswers": [
            {
              "ja": "アップルのです。",
              "vi": "Là của Apple."
            },
            {
              "ja": "アメリカのです。",
              "vi": "Là của Mỹ."
            }
          ],
          "relatedGrammar": [
            {
              "pattern": "N は どこのですか。",
              "meaning": "N là của hãng/nước nào?",
              "howToUse": "Dùng để hỏi xuất xứ hoặc hãng sản xuất."
            }
          ],
          "relatedVocabulary": {
            "topic": "Hãng và xuất xứ điện thoại",
            "items": [
              {
                "word": "どこの",
                "reading": "どこの",
                "meaning": "Của nước nào/hãng nào"
              },
              {
                "word": "アップル",
                "reading": "アップル",
                "meaning": "Apple"
              },
              {
                "word": "アメリカ",
                "reading": "アメリカ",
                "meaning": "Mỹ"
              },
              {
                "word": "日本",
                "reading": "にほん",
                "meaning": "Nhật Bản"
              },
              {
                "word": "ベトナム",
                "reading": "ベトナム",
                "meaning": "Việt Nam"
              },
              {
                "word": "サムスン",
                "reading": "サムスン",
                "meaning": "Samsung"
              },
              {
                "word": "の",
                "reading": "の",
                "meaning": "Của/thuộc về"
              }
            ]
          },
          "explanation": "Trong tranh ghi hãng là APPLE（アップル） và xuất xứ là アメリカ. Có thể trả lời アップルのです hoặc アメリカのです.",
          "tips": [
            "どこの có thể hỏi hãng hoặc nước.",
            "Tên hãng/nước + のです là câu trả lời ngắn.",
            "Không thêm 人 sau アメリカ vì đang nói đồ vật."
          ],
          "commonMistakes": [
            "Trả lời アンさんのです dù câu hỏi hỏi hãng/xuất xứ.",
            "Quên の sau アップル hoặc アメリカ.",
            "Trả lời アメリカ人です."
          ]
        },
        {
          "id": "l2_img_iphone_q05",
          "order": 5,
          "question": {
            "ja": "このアイフォンは　サムスンのですか。",
            "vi": "Chiếc iPhone này là của Samsung phải không?"
          },
          "sampleAnswers": [
            {
              "ja": "いいえ、アップルのです。",
              "vi": "Không, là của Apple."
            },
            {
              "ja": "いいえ、サムスンのじゃありません。アップルのです。",
              "vi": "Không, không phải của Samsung. Là của Apple."
            }
          ],
          "relatedGrammar": [
            {
              "pattern": "N は 会社のですか。",
              "meaning": "N có phải là của hãng đó không?",
              "howToUse": "Dùng để xác nhận hãng sản xuất."
            },
            {
              "pattern": "いいえ、Nじゃありません。Nです。",
              "meaning": "Không, không phải N. Là N.",
              "howToUse": "Dùng khi thông tin trong câu hỏi sai."
            }
          ],
          "relatedVocabulary": {
            "topic": "Xác nhận hãng",
            "items": [
              {
                "word": "サムスン",
                "reading": "サムスン",
                "meaning": "Samsung"
              },
              {
                "word": "アップル",
                "reading": "アップル",
                "meaning": "Apple"
              },
              {
                "word": "いいえ",
                "reading": "いいえ",
                "meaning": "Không"
              },
              {
                "word": "じゃありません",
                "reading": "じゃありません",
                "meaning": "Không phải"
              },
              {
                "word": "の",
                "reading": "の",
                "meaning": "Của"
              }
            ]
          },
          "explanation": "Trong tranh, iPhone là của アップル, không phải サムスン. Vì vậy trả lời いいえ、アップルのです。",
          "tips": [
            "Nếu thông tin trong câu hỏi sai, trả lời いいえ.",
            "サムスン và アップル đều là tên hãng.",
            "Tên hãng + のです là cách trả lời ngắn."
          ],
          "commonMistakes": [
            "Trả lời はい vì thấy đây là điện thoại.",
            "Nhầm サムスン với アップル.",
            "Quên の sau tên hãng."
          ]
        }
      ]
    },
    {
      "pictureId": "jpd113_l2_picture_04_cafe_order",
      "pictureTitle": "Tranh 4 - きっさてんで注文する",
      "pictureContentForGeneration": {
        "purpose": "Tạo tranh tình huống gọi món trong quán cà phê để luyện mẫu Nをください và số lượng.",
        "sceneDescription": {
          "place": "きっさてん",
          "items": [
            {
              "item": "ケーキ",
              "quantity": "ひとつ"
            },
            {
              "item": "コーヒー",
              "quantity": "ふたつ"
            }
          ],
          "visualRequirement": "Tranh có 1 miếng bánh kem và 2 tách cà phê trên bàn. Có thể có nhân viên quán cà phê đang chờ khách gọi món."
        },
        "japaneseInfoForAnswering": {
          "orderSentence": "ケーキを　ひとつと　コーヒーを　ふたつください。",
          "cakeQuantity": "ひとつ",
          "coffeeQuantity": "ふたつ",
          "place": "きっさてん"
        }
      },
      "questions": [
        {
          "id": "l2_img_cafe_q01",
          "order": 1,
          "question": {
            "ja": "ここは　どこですか。",
            "vi": "Đây là đâu?"
          },
          "sampleAnswers": [
            {
              "ja": "きっさてんです。",
              "vi": "Là quán cà phê."
            }
          ],
          "relatedGrammar": [
            {
              "pattern": "ここは N です。",
              "meaning": "Đây là N / ở đây là N.",
              "howToUse": "Dùng để trả lời địa điểm trong tranh."
            }
          ],
          "relatedVocabulary": {
            "topic": "Địa điểm ăn uống",
            "items": [
              {
                "word": "ここ",
                "reading": "ここ",
                "meaning": "Ở đây"
              },
              {
                "word": "どこ",
                "reading": "どこ",
                "meaning": "Ở đâu"
              },
              {
                "word": "きっさてん",
                "reading": "きっさてん",
                "meaning": "Quán cà phê"
              },
              {
                "word": "レストラン",
                "reading": "レストラン",
                "meaning": "Nhà hàng"
              },
              {
                "word": "スーパー",
                "reading": "スーパー",
                "meaning": "Siêu thị"
              }
            ]
          },
          "explanation": "Tranh là tình huống gọi món trong quán cà phê, nên trả lời きっさてんです。",
          "tips": [
            "ここはどこですか hỏi địa điểm.",
            "Trả lời bằng tên địa điểm + です。",
            "Không cần kể món trong câu này."
          ],
          "commonMistakes": [
            "Trả lời ケーキです dù câu hỏi hỏi địa điểm.",
            "Nhầm きっさてん với スーパー.",
            "Quên です."
          ]
        },
        {
          "id": "l2_img_cafe_q02",
          "order": 2,
          "question": {
            "ja": "ケーキは　いくつですか。",
            "vi": "Có mấy miếng bánh kem?"
          },
          "sampleAnswers": [
            {
              "ja": "ひとつです。",
              "vi": "Có 1 cái/phần."
            },
            {
              "ja": "ケーキは　ひとつです。",
              "vi": "Bánh kem có 1 phần."
            }
          ],
          "relatedGrammar": [
            {
              "pattern": "N は いくつですか。",
              "meaning": "N có mấy cái?",
              "howToUse": "Dùng để hỏi số lượng đồ vật/thức ăn."
            }
          ],
          "relatedVocabulary": {
            "topic": "Số lượng món ăn",
            "items": [
              {
                "word": "ケーキ",
                "reading": "ケーキ",
                "meaning": "Bánh kem"
              },
              {
                "word": "コーヒー",
                "reading": "コーヒー",
                "meaning": "Cà phê"
              },
              {
                "word": "ジュース",
                "reading": "ジュース",
                "meaning": "Nước ép"
              },
              {
                "word": "いくつ",
                "reading": "いくつ",
                "meaning": "Mấy cái"
              },
              {
                "word": "ひとつ",
                "reading": "ひとつ",
                "meaning": "Một cái/một phần"
              },
              {
                "word": "ふたつ",
                "reading": "ふたつ",
                "meaning": "Hai cái/hai phần"
              },
              {
                "word": "みっつ",
                "reading": "みっつ",
                "meaning": "Ba cái/ba phần"
              }
            ]
          },
          "explanation": "Trong tranh có 1 miếng bánh kem, nên trả lời ひとつです。",
          "tips": [
            "いくつ hỏi số lượng.",
            "ひとつ là 1 cái/1 phần.",
            "Có thể trả lời ngắn bằng số lượng + です。"
          ],
          "commonMistakes": [
            "Trả lời ケーキです dù câu hỏi hỏi số lượng.",
            "Nhầm ひとつ với ふたつ.",
            "Quên です cuối câu."
          ]
        },
        {
          "id": "l2_img_cafe_q03",
          "order": 3,
          "question": {
            "ja": "コーヒーは　いくつですか。",
            "vi": "Có mấy tách cà phê?"
          },
          "sampleAnswers": [
            {
              "ja": "ふたつです。",
              "vi": "Có 2 tách."
            },
            {
              "ja": "コーヒーは　ふたつです。",
              "vi": "Cà phê có 2 tách."
            }
          ],
          "relatedGrammar": [
            {
              "pattern": "N は いくつですか。",
              "meaning": "N có mấy cái?",
              "howToUse": "Dùng để hỏi số lượng đồ uống."
            }
          ],
          "relatedVocabulary": {
            "topic": "Đồ uống và số lượng",
            "items": [
              {
                "word": "コーヒー",
                "reading": "コーヒー",
                "meaning": "Cà phê"
              },
              {
                "word": "ジュース",
                "reading": "ジュース",
                "meaning": "Nước ép"
              },
              {
                "word": "みず",
                "reading": "みず",
                "meaning": "Nước"
              },
              {
                "word": "おちゃ",
                "reading": "おちゃ",
                "meaning": "Trà"
              },
              {
                "word": "いくつ",
                "reading": "いくつ",
                "meaning": "Mấy cái"
              },
              {
                "word": "ひとつ",
                "reading": "ひとつ",
                "meaning": "Một cái/một phần"
              },
              {
                "word": "ふたつ",
                "reading": "ふたつ",
                "meaning": "Hai cái/hai phần"
              },
              {
                "word": "みっつ",
                "reading": "みっつ",
                "meaning": "Ba cái/ba phần"
              }
            ]
          },
          "explanation": "Trong tranh có 2 tách cà phê, nên trả lời ふたつです。",
          "tips": [
            "Câu hỏi giống câu ケーキは いくつですか.",
            "Chỉ cần đổi đối tượng thành コーヒー.",
            "Trả lời bằng số lượng + です。"
          ],
          "commonMistakes": [
            "Nhầm số lượng cà phê với bánh kem.",
            "Trả lời コーヒーをください dù câu hỏi hỏi số lượng.",
            "Thiếu です."
          ]
        },
        {
          "id": "l2_img_cafe_q04",
          "order": 4,
          "question": {
            "ja": "ごちゅうもんを　どうぞ。",
            "vi": "Mời bạn gọi món."
          },
          "sampleAnswers": [
            {
              "ja": "ケーキを　ひとつと　コーヒーを　ふたつください。",
              "vi": "Cho tôi 1 phần bánh kem và 2 tách cà phê."
            }
          ],
          "relatedGrammar": [
            {
              "pattern": "N を ください。",
              "meaning": "Cho tôi N.",
              "howToUse": "Dùng khi gọi món hoặc mua hàng."
            },
            {
              "pattern": "N を 数 lượng と N を 数 lượng ください。",
              "meaning": "Cho tôi số lượng N và số lượng N.",
              "howToUse": "Dùng để gọi nhiều món có số lượng."
            }
          ],
          "relatedVocabulary": {
            "topic": "Gọi món",
            "items": [
              {
                "word": "ごちゅうもん",
                "reading": "ごちゅうもん",
                "meaning": "Gọi món/đơn gọi món"
              },
              {
                "word": "どうぞ",
                "reading": "どうぞ",
                "meaning": "Xin mời"
              },
              {
                "word": "ケーキ",
                "reading": "ケーキ",
                "meaning": "Bánh kem"
              },
              {
                "word": "コーヒー",
                "reading": "コーヒー",
                "meaning": "Cà phê"
              },
              {
                "word": "ジュース",
                "reading": "ジュース",
                "meaning": "Nước ép"
              },
              {
                "word": "みず",
                "reading": "みず",
                "meaning": "Nước"
              },
              {
                "word": "ひとつ",
                "reading": "ひとつ",
                "meaning": "Một cái/một phần"
              },
              {
                "word": "ふたつ",
                "reading": "ふたつ",
                "meaning": "Hai cái/hai phần"
              },
              {
                "word": "ください",
                "reading": "ください",
                "meaning": "Cho tôi/làm ơn"
              },
              {
                "word": "と",
                "reading": "と",
                "meaning": "Và"
              }
            ]
          },
          "explanation": "Trong tranh có 1 phần ケーキ và 2 tách コーヒー. Khi gọi món, dùng mẫu ケーキをひとつとコーヒーをふたつください。",
          "tips": [
            "ごちゅうもんをどうぞ là lời của nhân viên mời khách gọi món.",
            "Khi gọi món, dùng Nをください。",
            "Nếu có hai món, dùng と để nối.",
            "ひとつ là 1 phần, ふたつ là 2 phần."
          ],
          "commonMistakes": [
            "Quên を trước ください.",
            "Quên số lượng ひとつ／ふたつ.",
            "Dùng です thay vì ください khi đang gọi món."
          ]
        },
        {
          "id": "l2_img_cafe_q05",
          "order": 5,
          "question": {
            "ja": "ケーキを　ふたつくださいか。",
            "vi": "Bạn gọi 2 phần bánh kem phải không?"
          },
          "sampleAnswers": [
            {
              "ja": "いいえ、ケーキを　ひとつください。",
              "vi": "Không, cho tôi 1 phần bánh kem."
            },
            {
              "ja": "いいえ、ケーキは　ひとつです。",
              "vi": "Không, bánh kem là 1 phần."
            }
          ],
          "relatedGrammar": [
            {
              "pattern": "N を 数量 ください。",
              "meaning": "Cho tôi số lượng N.",
              "howToUse": "Dùng khi gọi món có số lượng."
            },
            {
              "pattern": "いいえ、Nを 数量 ください。",
              "meaning": "Không, cho tôi số lượng N.",
              "howToUse": "Dùng để sửa lại số lượng khi thông tin trong câu hỏi sai."
            }
          ],
          "relatedVocabulary": {
            "topic": "Sửa số lượng khi gọi món",
            "items": [
              {
                "word": "ケーキ",
                "reading": "ケーキ",
                "meaning": "Bánh kem"
              },
              {
                "word": "ひとつ",
                "reading": "ひとつ",
                "meaning": "Một cái/một phần"
              },
              {
                "word": "ふたつ",
                "reading": "ふたつ",
                "meaning": "Hai cái/hai phần"
              },
              {
                "word": "いいえ",
                "reading": "いいえ",
                "meaning": "Không"
              },
              {
                "word": "ください",
                "reading": "ください",
                "meaning": "Cho tôi/làm ơn"
              }
            ]
          },
          "explanation": "Trong tranh chỉ có 1 phần ケーキ, không phải 2 phần. Vì vậy trả lời いいえ、ケーキをひとつください。",
          "tips": [
            "Khi số lượng trong câu hỏi sai, trả lời いいえ.",
            "Sau đó nói lại số lượng đúng.",
            "ひとつ là 1, ふたつ là 2."
          ],
          "commonMistakes": [
            "Trả lời はい dù tranh chỉ có 1 phần bánh.",
            "Nhầm ひとつ và ふたつ.",
            "Quên を trước ください."
          ]
        }
      ]
    }
  ],
  "quickReview": {
    "note": "Tranh 1 giữ nguyên câu hỏi tầng theo data gốc. Các tranh còn lại đã đổi mới: sản phẩm túi/cặp, điện thoại iPhone, và gọi món bánh kem + cà phê.",
    "coreQuestionKeywords": [
      {
        "keyword": "どこ",
        "expectedAnswerType": "Vị trí hoặc tầng"
      },
      {
        "keyword": "なんかい",
        "expectedAnswerType": "Số tầng"
      },
      {
        "keyword": "いくら",
        "expectedAnswerType": "Giá tiền"
      },
      {
        "keyword": "だれの",
        "expectedAnswerType": "Người sở hữu"
      },
      {
        "keyword": "どこの",
        "expectedAnswerType": "Hãng/nước xuất xứ"
      },
      {
        "keyword": "いくつ",
        "expectedAnswerType": "Số lượng"
      },
      {
        "keyword": "ください",
        "expectedAnswerType": "Câu gọi món/mua hàng"
      }
    ]
  }
}
```
