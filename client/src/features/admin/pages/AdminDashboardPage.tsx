import { Link } from 'react-router-dom';
import { FileQuestion, Settings, Users, Wand2 } from 'lucide-react';

const ADMIN_PATH = '/jplearn-manage-xh21';

const cards = [
  {
    title: 'Câu hỏi luyện thi',
    description: 'Tạo, sửa, import nhanh câu hỏi theo từng khóa.',
    path: `${ADMIN_PATH}/exam-questions`,
    icon: FileQuestion,
    active: true,
  },
  {
    title: 'Người dùng',
    description: 'Quản lý tài khoản, role, gói premium.',
    path: `${ADMIN_PATH}/users`,
    icon: Users,
    active: true,
  },
  {
    title: 'Thông tin web',
    description: 'Banner, thông báo, cấu hình nội dung.',
    path: `${ADMIN_PATH}/site`,
    icon: Settings,
    active: false,
  },
];

export const AdminDashboardPage = () => (
  <div className="mx-auto max-w-7xl space-y-8 px-4 py-8 lg:px-8">
    <section className="rounded-2xl border border-slate-200 bg-white p-6 shadow-sm">
      <div className="flex flex-col gap-5 lg:flex-row lg:items-end lg:justify-between">
        <div>
          <p className="text-xs font-black uppercase tracking-[0.18em] text-blue-600">Admin dashboard</p>
          <h1 className="mt-2 font-heading text-4xl font-black text-slate-950">Quản trị nội dung JPLearn</h1>
          <p className="mt-3 max-w-3xl text-sm font-bold leading-6 text-slate-600">
            Dashboard này tách khỏi sidebar học tập và được tổ chức để mở rộng thêm user, premium, cấu hình website sau này.
          </p>
        </div>
        <Link
          to={`${ADMIN_PATH}/exam-questions`}
          className="inline-flex h-12 items-center justify-center gap-2 rounded-xl bg-blue-600 px-5 text-sm font-black text-white shadow-sm hover:bg-blue-700"
        >
          <Wand2 size={18} />
          Import câu hỏi
        </Link>
      </div>
    </section>

    <section className="grid gap-4 md:grid-cols-3">
      {cards.map((card) => {
        const Icon = card.icon;
        const body = (
          <div className={`h-full rounded-2xl border bg-white p-5 shadow-sm transition ${
            card.active ? 'border-slate-200 hover:-translate-y-0.5 hover:shadow-md' : 'border-slate-200 opacity-60'
          }`}>
            <div className="flex items-center justify-between">
              <span className={`grid h-11 w-11 place-items-center rounded-xl ${card.active ? 'bg-blue-100 text-blue-700' : 'bg-slate-100 text-slate-500'}`}>
                <Icon size={22} />
              </span>
              {!card.active ? <span className="rounded-full bg-slate-100 px-2 py-1 text-[10px] font-black uppercase text-slate-500">soon</span> : null}
            </div>
            <h2 className="mt-4 font-heading text-xl font-black text-slate-950">{card.title}</h2>
            <p className="mt-2 text-sm font-bold leading-6 text-slate-600">{card.description}</p>
          </div>
        );

        return card.active ? <Link key={card.title} to={card.path}>{body}</Link> : <div key={card.title}>{body}</div>;
      })}
    </section>
  </div>
);
