export interface QaLessonOverview {
  shortSummary: string;
  studentCanDo: string[];
  mainSkills: string[];
  mainGrammarFocus: string[];
  examTipSummary: string;
}

export interface QaLesson {
  id: number;
  title: string;
  description: string;
  overview?: QaLessonOverview;
  noImageDesc?: string;
  noImageQuestionCount?: number;
  withImageDesc?: string;
  withImagePictureCount?: number;
  withImageQuestionCount?: number;
}

export const jpd113QaLessons: QaLesson[] = [
  {
    id: 1,
    title: "Bài 1 - Giới thiệu bản thân cơ bản",
    description: "Tổng quan các câu hỏi cơ bản về tên, tuổi, quốc tịch, sinh nhật và sở thích.",
    noImageDesc: "Luyện phản xạ nói trực tiếp thông qua danh sách 17 câu hỏi giao tiếp cơ bản (tên, tuổi, sở thích, sinh nhật...).",
    noImageQuestionCount: 17,
    withImageDesc: "Quan sát hình vẽ nhân vật (An-san, Lee-san) và trả lời các câu hỏi vấn đáp xoay quanh thông tin trong tranh.",
    withImagePictureCount: 2,
    withImageQuestionCount: 17,
    overview: {
      shortSummary: "Sau Bài 1, sinh viên có thể trả lời ngắn gọn các câu hỏi cơ bản về tên, quốc gia, quốc tịch, nghề nghiệp, tuổi, sinh nhật và sở thích bằng mẫu câu danh từ đơn giản.",
      studentCanDo: [
        "Trả lời được tên của mình.",
        "Trả lời được mình đến từ nước nào và là người nước nào.",
        "Trả lời được nghề nghiệp/trạng thái hiện tại.",
        "Trả lời được tuổi.",
        "Trả lời được sinh nhật theo tháng hoặc theo tháng/ngày.",
        "Trả lời được sở thích cá nhân."
      ],
      mainSkills: [
        "Nghe câu hỏi ngắn và nhận ra từ khóa chính.",
        "Trả lời bằng câu ngắn đúng ngữ pháp.",
        "Dùng はい／いいえ cho câu hỏi xác nhận.",
        "Dùng Nです／Nじゃありません để trả lời câu danh từ."
      ],
      mainGrammarFocus: [
        "N1 は N2 です。",
        "N1 は N2 じゃありません。",
        "N1 は N2 ですか。",
        "N は？",
        "N は なんですか。",
        "N1 の N2 は N3 です。",
        "N1 と N2"
      ],
      examTipSummary: "Khi thi vấn đáp, sinh viên không cần trả lời dài. Chỉ cần nghe đúng từ khóa như おなまえ, おくに, おしごと, なんさい, たんじょうび, しゅみ và trả lời bằng mẫu ngắn + です."
    }
  },
  {
    id: 2,
    title: "Bài 2 - Vị trí, giá cả và giải thích đồ vật",
    description: "Tổng quan các câu hỏi về vị trí, tầng, giá tiền, đồ vật, sở hữu và xuất xứ.",
    noImageDesc: "Luyện phản xạ nói trực tiếp thông qua các câu hỏi về vị trí, tầng, giá tiền, sở hữu và giải thích đồ vật.",
    noImageQuestionCount: 17,
    withImageDesc: "Quan sát sơ đồ tầng tòa nhà mua sắm và thẻ thông tin sản phẩm (túi xách, iphone, quán cà phê) để trả lời các câu hỏi.",
    withImagePictureCount: 4,
    withImageQuestionCount: 19,
    overview: {
      shortSummary: "Sau Bài 2, sinh viên có thể trả lời ngắn gọn các câu hỏi cơ bản về vị trí, tầng, giá tiền, đồ vật, sở hữu, xuất xứ và cách nói một từ bằng ngôn ngữ khác.",
      studentCanDo: [
        "Trả lời được địa điểm hoặc đồ vật ở đâu.",
        "Trả lời được một địa điểm ở tầng mấy.",
        "Xác nhận được một địa điểm có ở đúng vị trí được hỏi hay không.",
        "Hỏi và trả lời được giá tiền bằng ドン, えん, ドル.",
        "Nói được đồ vật này là gì.",
        "Trả lời được đồ vật là của ai.",
        "Trả lời được đồ vật là của nước nào hoặc hãng nào.",
        "Hỏi và trả lời được một từ trong tiếng Nhật, tiếng Anh hoặc tiếng Việt là gì."
      ],
      mainSkills: [
        "Nghe câu hỏi ngắn và nhận ra từ khóa chính như どこ, なんかい, いくら, だれの, どこの.",
        "Trả lời bằng câu ngắn đúng ngữ pháp.",
        "Dùng はい／いいえ cho câu hỏi xác nhận.",
        "Dùng ちがいます khi thông tin trong câu hỏi sai.",
        "Dùng số tiền và đơn vị tiền tệ đúng sau danh từ.",
        "Thay đổi từ vựng cùng nhóm để tạo nhiều câu trả lời khác nhau."
      ],
      mainGrammarFocus: [
        "N は どこですか。",
        "N は なんかいですか。",
        "N は ここ／そこ／あそこですか。",
        "N は いくらですか。",
        "N は だれの N ですか。",
        "N は どこの N ですか。",
        "「A」は Bごで なんですか。"
      ],
      examTipSummary: "Khi thi vấn đáp, sinh viên không cần trả lời dài. Chỉ cần nghe đúng từ khóa như どこ, なんかい, いくら, だれの, どこの và trả lời bằng mẫu ngắn + です. Với câu hỏi Yes/No, nên bắt đầu bằng はい hoặc いいえ."
    }
  },
  {
    id: 3,
    title: "Bài 3 - Thời gian, kế hoạch và hoạt động hằng ngày",
    description: "Tổng quan các câu hỏi về giờ giấc, thời gian mở cửa, kế hoạch ngày nghỉ, địa điểm đi đến và hoạt động hằng ngày.",
    noImageDesc: "Luyện phản xạ nói trực tiếp thông qua các câu hỏi về giờ giấc, kế hoạch ngày nghỉ và hoạt động hằng ngày.",
    noImageQuestionCount: 20,
    withImageDesc: "Quan sát sơ đồ lịch trình tháng 11, bảng giờ mở cửa thư viện/tiệm bánh và tranh hoạt động nhân vật để trả lời các câu hỏi.",
    withImagePictureCount: 5,
    withImageQuestionCount: 25,
    overview: {
      shortSummary: "Sau Bài 3, sinh viên có thể trả lời ngắn gọn các câu hỏi về giờ giấc, thời gian mở cửa, kế hoạch ngày nghỉ, địa điểm đi đến và hoạt động hằng ngày bằng động từ thể ます／ません.",
      studentCanDo: [
        "Trả lời được bây giờ là mấy giờ.",
        "Hỏi và trả lời được một địa điểm mở cửa từ mấy giờ đến mấy giờ.",
        "Trả lời được ngày nghỉ sẽ làm gì.",
        "Trả lời được sẽ đi đâu vào kỳ nghỉ.",
        "Trả lời được hoạt động hằng ngày.",
        "Nói được hành động xảy ra ở đâu bằng mẫu NでVます.",
        "Nói được hành động xảy ra lúc mấy giờ bằng mẫu NにVます.",
        "Dùng はい／いいえ để trả lời câu hỏi xác nhận với động từ."
      ],
      mainSkills: [
        "Nghe từ khóa và nhận diện các trạng từ chỉ tần suất như まいにち, まいあさ, まいばん.",
        "Trả lời bằng câu ngắn đúng động từ thể ます／ません.",
        "Phân biệt câu hỏi thời gian, địa điểm và hành động.",
        "Dùng trợ từ を cho tân ngữ trực tiếp.",
        "Dùng trợ từ で cho nơi diễn ra hành động.",
        "Dùng trợ từ へ cho nơi đi đến.",
        "Dùng trợ từ に với thời gian cụ thể."
      ],
      mainGrammarFocus: [
        "Vます／Vません。",
        "Nへ　いきます。",
        "Nを　Vます。",
        "Nに　Vます。",
        "Nで　Vます。",
        "N1から　N2までです。",
        "どこも　Vません。",
        "はい、Vます。／いいえ、Vません。"
      ],
      examTipSummary: "Khi thi vấn đáp Bài 3, sinh viên cần nghe đúng từ khóa. Nếu nghe どこへ thì trả lời nơi đi đến. Nếu nghe どこで thì trả lời nơi diễn ra hành động. Nếu nghe なにをしますか thì trả lời bằng hành động + ます."
    }
  }
];

export const jpd123QaLessons: QaLesson[] = [
  {
    id: 4,
    title: "Bài 4 - Thành phố, phương tiện, thời tiết và món ăn",
    description: "Tổng quan các câu hỏi về quê hương, phương tiện, thời gian di chuyển, thời tiết và hương vị món ăn.",
    noImageDesc: "Luyện phản xạ nói trực tiếp thông qua các câu hỏi về quê quán, phương tiện di chuyển, thời tiết và món ăn.",
    noImageQuestionCount: 19,
    withImageDesc: "Quan sát tranh bản đồ và các thẻ thông tin địa điểm để trả lời câu hỏi về vị trí, phương tiện, thời tiết và món ăn.",
    withImagePictureCount: 7,
    withImageQuestionCount: 73,
    overview: {
      shortSummary: "Sau Bài 4, sinh viên có thể hỏi và trả lời về vị trí thành phố/quốc gia, thời gian di chuyển, phương tiện đi lại, miêu tả thành phố hoặc địa điểm, nói nơi đó có gì, nói về thời tiết và cảm nhận món ăn bằng tiếng Nhật sơ cấp.",
      studentCanDo: [
        "Nói được thành phố/quê của mình là ở đâu.",
        "Nói được một thành phố nằm ở phía bắc, nam, đông, tây hoặc chính giữa của một quốc gia.",
        "Hỏi và trả lời được từ nơi này đến nơi khác mất bao lâu.",
        "Hỏi và trả lời được đi bằng phương tiện gì.",
        "Miêu tả được thành phố, trường học, địa điểm bằng tính từ đuôi い và tính từ đuôi な.",
        "Nói được ở một thành phố/địa điểm có gì bằng mẫu あります.",
        "Hỏi và trả lời được thời tiết hoặc cảm nhận về món ăn bằng どうですか."
      ],
      mainSkills: [
        "Nghe câu hỏi và nhận ra từ khóa chính như どこ, どのくらい, なんで, どんな, ありますか, どうですか.",
        "Trả lời ngắn, đúng trọng tâm bằng mẫu câu sơ cấp.",
        "Dùng はい／いいえ cho câu hỏi xác nhận.",
        "Dùng tính từ đúng dạng khi khẳng định, phủ định và bổ nghĩa cho danh từ.",
        "Dùng từ nối và phó từ mức độ như とても, すこし, あまり.",
        "Dùng Nでいきます để nói phương tiện và あるいていきます để nói đi bộ."
      ],
      mainGrammarFocus: [
        "Nは　N của どこですか。",
        "Nから　Nまで　どのくらいですか。",
        "Nで　いきます。",
        "あるいて　いきます。",
        "Nは　Aいです. / Aくないです.",
        "Nは　Aです. / Aじゃありません.",
        "AいN / AなN",
        "Nに　Nが　あります。",
        "Nは　どうですか。"
      ],
      examTipSummary: "Khi thi vấn đáp Bài 4, sinh viên cần nghe đúng từ khóa: どこ hỏi vị trí, どのくらい hỏi thời gian, なんで hỏi phương tiện, どんな hỏi đặc điểm, なにがありますか hỏi có gì, どうですか hỏi tình trạng/cảm nhận. Trả lời ngắn bằng mẫu câu + です là đủ."
    }
  },
  {
    id: 5,
    title: "Bài 5 - Quá khứ, cảm nhận, sở thích và mong muốn",
    description: "Tổng quan các câu hỏi về hoạt động trong quá khứ, cảm nhận, sở thích và mong muốn.",
    noImageDesc: "Luyện phản xạ nói trực tiếp thông qua các câu hỏi về quá khứ, cảm nhận, sở thích và mong muốn.",
    noImageQuestionCount: 36,
    withImageDesc: "Quan sát tranh hoạt động ngày nghỉ và đồ vật mong muốn để trả lời các câu hỏi tương ứng.",
    withImagePictureCount: 9,
    withImageQuestionCount: 39,
    overview: {
      shortSummary: "Sau Bài 5, sinh viên có thể hỏi và trả lời về việc đã làm trong quá khứ, cảm nhận về sự việc trong quá khứ, sở thích, mong muốn có gì, muốn làm gì và kế hoạch cho ngày nghỉ.",
      studentCanDo: [
        "Hỏi và trả lời được: ngày nghỉ đã làm gì.",
        "Nói được đã đi đâu, làm gì, làm với ai, làm ở đâu.",
        "Trả lời được câu hỏi Yes/No ở quá khứ bằng ました／ませんでした.",
        "Nói được cảm nhận trong quá khứ bằng tính từ quá khứ như たのしかったです／たいへんでした.",
        "Diễn đạt được thích gì, không thích gì.",
        "Diễn đạt được muốn có gì bằng ほしいです.",
        "Diễn đạt được muốn làm gì bằng Vたいです.",
        "Nói được kế hoạch cho cuối tuần hoặc kỳ nghỉ."
      ],
      mainSkills: [
        "Nghe từ khóa thời gian quá khứ như きのう, おととい, せんしゅう, しゅうまつ.",
        "Phân biệt câu hỏi hành động quá khứ và câu hỏi cảm nhận quá khứ.",
        "Trả lời ngắn, đúng trọng tâm khi vấn đáp.",
        "Dùng đúng trợ từ で, へ, と, に, が, を."
      ],
      mainGrammarFocus: [
        "Vました／Vませんでした",
        "Nで Vました",
        "Nへ いきました",
        "Nと Vました",
        "どこかへ いきましたか",
        "どこへも いきませんでした",
        "い-adj かったです／くなかったです",
        "な-adj／N でした／じゃありませんでした",
        "Nが すきです／すきじゃありません",
        "Nが ほしいです／なにも ほしくないです",
        "Vたいです",
        "Nへ Vに いきます"
      ],
      examTipSummary: "Khi thi vấn đáp Bài 5, nếu câu hỏi có しましたか thì trả lời bằng しました／しませんでした. Nếu câu hỏi có どうでしたか thì trả lời cảm nhận như たのしかったです, おもしろかったです, たいへんでした. Nếu có ほしい thì trả lời đồ muốn có + がほしいです. Nếu có たい thì trả lời động từ bỏ ます + たいです."
    }
  },
  {
    id: 6,
    title: "Bài 6 - Lời rủ rê, so sánh và cuộc hẹn",
    description: "Tổng quan các câu hỏi về lời rủ rê cùng làm gì, so sánh hơn, so sánh nhất và thiết lập cuộc hẹn.",
    noImageDesc: "Luyện phản xạ nói trực tiếp thông qua các câu hỏi về lời rủ rê, so sánh và cuộc hẹn.",
    noImageQuestionCount: 27,
    withImageDesc: "Quan sát các tranh tình huống để thực hiện hội thoại rủ rê, so sánh các địa điểm/món ăn và lên lịch cuộc hẹn.",
    withImagePictureCount: 6,
    withImageQuestionCount: 31,
    overview: {
      shortSummary: "Sau Bài 6, sinh viên có thể thực hiện hội thoại rủ rê cùng làm gì, chấp nhận/từ chối lời mời lịch sự, so sánh hơn/so sánh nhất và chốt cuộc hẹn.",
      studentCanDo: [
        "Rủ bạn cùng làm gì bằng mẫu Vませんか.",
        "Đề nghị cùng làm gì bằng mẫu Vましょう.",
        "Nhận lời rủ rê bằng いいですね và từ chối lịch sự bằng すみません、ちょっと….",
        "Nói được mình có cuộc hẹn/công việc bằng Nがあります.",
        "Hỏi và trả lời về so sánh nhất trong một phạm vi bằng いちばん.",
        "Hỏi và so sánh hai đối tượng bằng どちら và ほう.",
        "Nói về việc đã hoàn thành hành động hay chưa bằng mẫu もうVましたか.",
        "Hỏi và chốt thời gian, địa điểm cuộc hẹn."
      ],
      mainSkills: [
        "Nghe hiểu và nhận diện đúng các lời mời, rủ rê.",
        "Từ chối lời mời một cách khéo léo và lịch sự.",
        "Sử dụng đúng cấu trúc so sánh hơn (より) và so sánh nhất (いちばん).",
        "Lên lịch và xác nhận lại cuộc hẹn."
      ],
      mainGrammarFocus: [
        "Vませんか／Vましょう",
        "Nが あります",
        "Nで Nがあります",
        "Nで Nがいちばん Aですか",
        "N1と N2と どちらが Aですか",
        "Nのほうが Aです",
        "N1は N2より Aです",
        "どこで 会いますか。"
      ],
      examTipSummary: "Khi thi vấn đáp Bài 6, hãy chú ý nghe đúng từ khóa. Nếu là lời mời (しませんか), hãy trả lời bằng mẫu đề nghị/đồng ý (しましょう) hoặc từ chối lịch sự. Nếu là so sánh (どちらが), hãy trả lời bằng Nのほうが Aです。"
    }
  },
  {
    id: 7,
    title: "Bài 7 - Sự tồn tại, nhờ vả và cách làm",
    description: "Tổng quan các câu hỏi về sự tồn tại của vật/người/động vật, nhờ vả làm việc gì và cách thức làm việc.",
    noImageDesc: "Luyện phản xạ nói trực tiếp thông qua các câu hỏi về vị trí tồn tại, nhờ vả, trạng thái đang làm và mang theo vật gì.",
    noImageQuestionCount: 25,
    withImageDesc: "Quan sát tranh sơ đồ phòng, hoạt động của nhân vật để trả lời về vị trí, nhờ vả và hành động đang làm.",
    withImagePictureCount: 6,
    withImageQuestionCount: 40,
    overview: {
      shortSummary: "Sau Bài 7, sinh viên có thể miêu tả vị trí đồ vật, con người, động vật, nhờ vả người khác làm gì, nói hành động đang diễn ra và cách thức thực hiện.",
      studentCanDo: [
        "Hỏi và trả lời vị trí của người/động vật bằng います và đồ vật bằng あります.",
        "Sử dụng các danh từ chỉ vị trí như まえ, うしろ, となり, ちかく để chỉ vị trí cụ thể.",
        "Đưa ra yêu cầu/nhờ vả một cách lịch sự bằng mẫu Vてください.",
        "Nói được hành động đang diễn ra bằng mẫu Vています.",
        "Đề nghị trợ giúp người khác bằng mẫu Vましょうか.",
        "Hỏi và chỉ cách làm một việc gì đó bằng mẫu Vかた.",
        "Nói được công cụ/ phương tiện thực hiện hành động bằng NでVます.",
        "Hỏi và trả lời về việc mang theo vật gì di chuyển bằng もって行きます và もって来ます。"
      ],
      mainSkills: [
        "Nghe hiểu các câu hỏi về vị trí tồn tại của vật/người.",
        "Đưa ra các câu nhờ vả hoặc đáp lại lời đề nghị giúp đỡ.",
        "Mô tả chính xác trạng thái đang diễn ra trong tranh."
      ],
      mainGrammarFocus: [
        "Nに Nが あります／います",
        "Vてください",
        "Vています",
        "Vましょうか",
        "Nの Vかた",
        "Nで Vます",
        "もって行きます／もって来ます"
      ],
      examTipSummary: "Khi gặp câu hỏi về vị trí (どこにありますか), hãy định vị đúng các địa điểm trên tranh. Khi nghe hỏi về phương thức (なにで...か), hãy trả lời rõ dụng cụ bằng で. Khi nghe hỏi hành động đang diễn ra (していますか), chú ý chia động từ thể ています."
    }
  }
];
