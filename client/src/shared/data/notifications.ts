export type NotificationIcon = 'message' | 'sparkles';

export interface AppNotification {
  id: number;
  icon: NotificationIcon;
  iconBg: string;
  iconColor: string;
  title: string;
  message: string;
  description: string;
  relatedLink?: string;
  relatedLinkLabel?: string;
  badges?: string[];
  highlights?: string[];
  gallery?: {
    src: string;
    alt: string;
    caption: string;
  }[];
  pricing?: {
    label: string;
    price: string;
    tone: 'blue' | 'emerald';
  }[];
  purchaseSteps?: string[];
  paymentImage?: string;
  contactLinks?: {
    label: string;
    href: string;
    tone: 'blue' | 'slate';
  }[];
  time: string;
}

export const NOTIFICATIONS: AppNotification[] = [
  {
    id: 4,
    icon: 'sparkles',
    iconBg: 'bg-rose-50/80',
    iconColor: 'text-rose-600',
    title: 'Ưu đãi source PE SWT301',
    message: 'Source PE SWT301 đúng format Q1, Q2, Q3: học nhanh hơn, làm bài chắc hơn.',
    description:
      'Nếu bạn đang học SWT301, đây là bộ source PE được soạn theo đúng format đề, có hướng dẫn chi tiết từng phần và chữa đề các kỳ rất kỹ. Gói đơn source SWT301 giá 15k. Nếu bạn đã mua combo trước đó, bạn được mua source SWT301 với giá ưu đãi 10k.',
    badges: ['SWT301', 'PE Source', 'Q1 Q2 Q3', 'Ưu đãi 10k'],
    highlights: [
      'Q1: List đủ bảng 16 lỗi thường gặp, kèm cách điền có sẵn. Bạn chỉ cần tìm lỗi và copy đúng format.',
      'Q2: Hướng dẫn làm White-box test chi tiết, giải thích cách phủ statement/branch và điền form test case.',
      'Q3: Hướng dẫn Black-box chi tiết, có template bảng EP, BVA, DT để phủ test case dễ hơn.',
      'Chữa đề các kỳ siêu chi tiết, giúp bạn nhìn được cách trình bày và tránh lỗi format khi nộp bài.',
    ],
    gallery: [
      {
        src: '/images/notifications/swt301/source-q1-theory.webp',
        alt: 'SWT301 Q1 code review và bug identification',
        caption: 'Q1 - Lý thuyết lỗi thường gặp và format trình bày',
      },
      {
        src: '/images/notifications/swt301/source-q2-whitebox.webp',
        alt: 'SWT301 Q2 white-box testing',
        caption: 'Q2 - Cách làm White-box test và form test case',
      },
      {
        src: '/images/notifications/swt301/source-q3-blackbox.webp',
        alt: 'SWT301 Q3 black-box testing',
        caption: 'Q3 - Black-box test, EP, BVA, Decision Table',
      },
      {
        src: '/images/notifications/swt301/source-q1-sample.webp',
        alt: 'SWT301 Q1 sample issue table',
        caption: 'Mẫu chữa Q1 theo bảng lỗi',
      },
      {
        src: '/images/notifications/swt301/source-q3-dt.webp',
        alt: 'SWT301 decision table template',
        caption: 'Template Decision Table phủ test',
      },
      {
        src: '/images/notifications/swt301/source-q3-template.webp',
        alt: 'SWT301 BVA and test case template',
        caption: 'Template BVA và bảng test case',
      },
    ],
    pricing: [
      { label: 'Đã mua gói đơn JPD', price: '15k', tone: 'blue' },
      { label: 'Đã mua combo', price: '10k', tone: 'emerald' },
    ],
    purchaseSteps: [
      'Chuyển khoản theo mã QR bên dưới.',
      'Sau khi chuyển khoản, nhắn email tài khoản học qua Zalo hoặc Facebook của mình.',
      'Mình sẽ xác nhận và gửi source PE SWT301 cho bạn.',
    ],
    paymentImage: '/images/notifications/swt301/payment-qr-code.webp',
    contactLinks: [
      { label: 'Nhắn Zalo', href: 'https://zalo.me/0833283840', tone: 'blue' },
      { label: 'Nhắn Facebook', href: 'https://www.facebook.com/xunhuns/', tone: 'slate' },
    ],
    time: 'Mới',
  },
  {
    id: 3,
    icon: 'message',
    iconBg: 'bg-emerald-50/80',
    iconColor: 'text-emerald-600',
    title: 'Mentor & Mẹo thi SE',
    message: 'Mentor các môn SE từ kỳ 1 - 4, Mẹo Tips Thi Ib Facebook zalo',
    description:
      'Nếu bạn cần người kèm định hướng các môn SE từ kỳ 1 đến kỳ 4, phần này dành để trao đổi lịch học, cách ôn, mẹo xử lý bài thi và các lưu ý khi học theo từng môn. Bạn có thể nhắn Zalo để hỏi chi tiết về môn đang vướng hoặc cần mentor hỗ trợ.',
    relatedLink: 'https://zalo.me/0833283840',
    relatedLinkLabel: 'Nhắn Zalo để hỏi mentor',
    time: 'Mới',
  },
  {
    id: 1,
    icon: 'message',
    iconBg: 'bg-blue-50/80',
    iconColor: 'text-blue-600',
    title: 'Hỗ trợ Speaking',
    message: 'Sắp tới bạn nào yếu phần speaking thì có thể ib zalo facebook mình nhận mentor hỗ trợ',
    description:
      'Thông báo dành cho các bạn đang yếu phần speaking hoặc muốn có người luyện phản xạ nói, sửa cách trả lời và hướng dẫn luyện theo bài. Khi cần hỗ trợ riêng, bạn có thể nhắn Zalo để trao đổi tình trạng hiện tại và lịch mentor phù hợp.',
    relatedLink: 'https://zalo.me/0833283840',
    relatedLinkLabel: 'Nhắn Zalo để luyện speaking',
    time: 'Mới',
  },
  {
    id: 2,
    icon: 'sparkles',
    iconBg: 'bg-amber-50/80',
    iconColor: 'text-amber-600',
    title: 'Tài liệu & Khóa học',
    message: 'Phần khóa học đã bao gồm source, các bài thi nói, tips luyện nói .... đừng ngần ngại mua :)',
    description:
      'Khóa học hiện có kèm source học tập, bài thi nói, tips luyện nói và các nội dung ôn tập để bạn học chủ động hơn trong app. Bạn có thể xem trang nâng cấp để biết các quyền lợi đang được mở cho tài khoản.',
    relatedLink: '/pricing',
    relatedLinkLabel: 'Xem gói khóa học',
    time: 'Mới',
  },
];

export const FIRST_NOTIFICATION_ID = NOTIFICATIONS[0]?.id ?? 1;
