import { useCallback, useEffect, useMemo, useState } from 'react';
import { CheckCircle2, Clipboard, FileJson, FileQuestion, Loader2, Plus, RotateCcw, Save, Search, Trash2, Upload } from 'lucide-react';
import { adminExamApi } from '../api/adminExamApi';
import type { AdminExamImportResult, AdminExamQuestion } from '../types/adminExam.types';
import {
  COURSE_OPTIONS,
  LEVEL_OPTIONS,
  TOPIC_OPTIONS,
  TYPE_OPTIONS,
  emptyQuestionForm,
  toQuestionForm,
  toQuestionPayload,
  type OptionForm,
  type QuestionForm,
} from '../utils/adminExamQuestionForm';

export const AdminExamQuestionsPage = () => {
  const [questions, setQuestions] = useState<AdminExamQuestion[]>([]);
  const [form, setForm] = useState<QuestionForm>(() => emptyQuestionForm());
  const [courseFilter, setCourseFilter] = useState('jpd113');
  const [topicFilter, setTopicFilter] = useState('');
  const [includeInactive, setIncludeInactive] = useState(false);
  const [query, setQuery] = useState('');
  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);
  const [isImporting, setIsImporting] = useState(false);
  const [importJsonText, setImportJsonText] = useState('');
  const [importResult, setImportResult] = useState<AdminExamImportResult | null>(null);
  const [message, setMessage] = useState('');
  const [error, setError] = useState('');

  const loadQuestions = useCallback(async () => {
    try {
      setIsLoading(true);
      setError('');
      const data = await adminExamApi.getQuestions({
        courseCode: courseFilter,
        topic: topicFilter || undefined,
        includeInactive,
      });
      setQuestions(data);
      if (form.id) {
        const updated = data.find((item) => item.id === form.id);
        if (updated) setForm(toQuestionForm(updated));
      }
    } catch (err) {
      console.error(err);
      setError('Không tải được danh sách câu hỏi.');
    } finally {
      setIsLoading(false);
    }
  }, [courseFilter, topicFilter, includeInactive, form.id]);

  useEffect(() => {
    const timer = window.setTimeout(() => {
      void loadQuestions();
    }, 0);

    return () => window.clearTimeout(timer);
  }, [loadQuestions]);

  const filteredQuestions = useMemo(() => {
    const normalized = query.trim().toLowerCase();
    if (!normalized) return questions;
    return questions.filter((question) =>
      [question.questionText, question.explanation, question.topic, question.level]
        .some((value) => value.toLowerCase().includes(normalized))
    );
  }, [questions, query]);

  const selectedQuestion = form.id ? questions.find((question) => question.id === form.id) : null;

  const updateForm = <K extends keyof QuestionForm>(key: K, value: QuestionForm[K]) => {
    setForm((current) => ({ ...current, [key]: value }));
  };

  const updateOption = (index: number, patch: Partial<OptionForm>) => {
    setForm((current) => ({
      ...current,
      options: current.options.map((option, optionIndex) =>
        optionIndex === index ? { ...option, ...patch } : option
      ),
    }));
  };

  const markCorrect = (index: number) => {
    setForm((current) => ({
      ...current,
      options: current.options.map((option, optionIndex) => ({
        ...option,
        isCorrect: optionIndex === index,
      })),
    }));
  };

  const resetForm = () => {
    setForm(emptyQuestionForm());
    setMessage('');
    setError('');
  };

  const saveQuestion = async () => {
    try {
      setIsSaving(true);
      setError('');
      setMessage('');
      const payload = toQuestionPayload(form);
      const saved = form.id
        ? await adminExamApi.updateQuestion(form.id, payload)
        : await adminExamApi.createQuestion(payload);
      setForm(toQuestionForm(saved));
      setCourseFilter(saved.courseCode);
      setTopicFilter(saved.topic);
      setMessage(form.id ? 'Đã cập nhật câu hỏi.' : 'Đã tạo câu hỏi.');
      await loadQuestions();
    } catch (err) {
      console.error(err);
      setError('Không lưu được câu hỏi. Kiểm tra nội dung, đáp án đúng và passage.');
    } finally {
      setIsSaving(false);
    }
  };

  const deleteQuestion = async () => {
    if (!form.id || !window.confirm('Ẩn câu hỏi này khỏi module luyện thi?')) return;

    try {
      setIsSaving(true);
      setError('');
      await adminExamApi.deleteQuestion(form.id);
      resetForm();
      setMessage('Đã ẩn câu hỏi khỏi luyện thi.');
      await loadQuestions();
    } catch (err) {
      console.error(err);
      setError('Không xóa được câu hỏi.');
    } finally {
      setIsSaving(false);
    }
  };

  const loadImportTemplate = async (type: 'standard' | 'reading') => {
    try {
      setError('');
      const template = await adminExamApi.getImportTemplate(type);
      setImportJsonText(JSON.stringify(template, null, 2));
    } catch (err) {
      console.error(err);
      setError('Không tải được mẫu JSON.');
    }
  };

  const importQuestions = async () => {
    try {
      setIsImporting(true);
      setError('');
      setMessage('');
      setImportResult(null);
      const payload = JSON.parse(importJsonText);
      const result = await adminExamApi.importJson(payload);
      setImportResult(result);
      setCourseFilter(payload.courseCode ?? courseFilter);
      setMessage(`Đã import ${result.totalQuestions} câu hỏi.`);
      await loadQuestions();
    } catch (err) {
      console.error(err);
      setError('Import thất bại. Kiểm tra JSON, courseCode, passageLocalId và đáp án đúng.');
    } finally {
      setIsImporting(false);
    }
  };

  return (
    <div className="mx-auto max-w-[1440px] space-y-6 px-4 py-6 lg:px-8 animate-fade-in">
      <header className="flex flex-col gap-4 lg:flex-row lg:items-end lg:justify-between">
        <div>
          <p className="text-xs font-black uppercase tracking-[0.18em] text-text-muted">Admin</p>
          <h1 className="mt-1 text-4xl font-black tracking-normal text-slate-900">Quản lý câu hỏi luyện thi</h1>
          <p className="mt-2 max-w-3xl text-sm font-bold leading-6 text-slate-600">
            Tạo, chỉnh sửa, ẩn câu hỏi và import nhanh bằng JSON cho từng khóa JPD113 / JPD123.
          </p>
        </div>

        <button
          type="button"
          onClick={resetForm}
          className="inline-flex h-12 items-center justify-center gap-2 border-2 border-slate-900 bg-[#2563EB] px-5 text-sm font-black text-white shadow-[4px_4px_0_#111827] transition-all hover:translate-x-[2px] hover:translate-y-[2px] hover:shadow-[2px_2px_0_#111827]"
        >
          <Plus size={18} />
          Câu hỏi mới
        </button>
      </header>

      <section className="grid gap-4 rounded-2xl border border-slate-200 bg-white p-5 shadow-sm xl:grid-cols-[0.9fr_1.1fr]">
        <div>
          <div className="flex items-center gap-3">
            <span className="grid h-11 w-11 place-items-center rounded-xl bg-blue-100 text-blue-700">
              <FileJson size={22} />
            </span>
            <div>
              <p className="text-xs font-black uppercase tracking-[0.16em] text-blue-600">Fast import</p>
              <h2 className="font-heading text-2xl font-black text-slate-950">Import nhanh bằng JSON</h2>
            </div>
          </div>
          <p className="mt-3 text-sm font-bold leading-6 text-slate-600">
            Dùng mẫu riêng cho câu thường hoặc reading. Câu hỏi trùng sẽ được cập nhật theo nội dung câu hỏi, `passageLocalId`
            chỉ dùng cho reading để nhiều câu hỏi dùng chung một đoạn văn.
          </p>
          <div className="mt-4 flex flex-wrap gap-2">
            <button
              type="button"
              onClick={() => loadImportTemplate('standard')}
              className="inline-flex h-10 items-center gap-2 rounded-lg border border-slate-300 bg-white px-4 text-sm font-black text-slate-800 hover:bg-slate-50"
            >
              <Clipboard size={16} />
              Mẫu câu thường
            </button>
            <button
              type="button"
              onClick={() => loadImportTemplate('reading')}
              className="inline-flex h-10 items-center gap-2 rounded-lg border border-blue-200 bg-blue-50 px-4 text-sm font-black text-blue-700 hover:bg-blue-100"
            >
              <FileJson size={16} />
              Mẫu reading
            </button>
            <button
              type="button"
              onClick={importQuestions}
              disabled={isImporting || !importJsonText.trim()}
              className="inline-flex h-10 items-center gap-2 rounded-lg bg-blue-600 px-4 text-sm font-black text-white hover:bg-blue-700 disabled:pointer-events-none disabled:opacity-60"
            >
              {isImporting ? <Loader2 size={16} className="animate-spin" /> : <Upload size={16} />}
              Import
            </button>
          </div>
          {importResult ? (
            <div className="mt-4 grid gap-2 text-sm font-black text-slate-700 sm:grid-cols-3">
              <span className="rounded-lg bg-slate-100 px-3 py-2">Passage: {importResult.importedPassages}</span>
              <span className="rounded-lg bg-green-100 px-3 py-2 text-green-700">Tạo mới: {importResult.createdQuestions}</span>
              <span className="rounded-lg bg-blue-100 px-3 py-2 text-blue-700">Cập nhật: {importResult.updatedQuestions}</span>
            </div>
          ) : null}
        </div>

        <textarea
          value={importJsonText}
          onChange={(event) => setImportJsonText(event.target.value)}
          rows={12}
          spellCheck={false}
          placeholder="Dán JSON theo mẫu ở đây..."
          className="min-h-72 w-full rounded-xl border border-slate-300 bg-slate-950 p-4 font-mono text-xs font-bold leading-5 text-slate-100 outline-none focus:border-blue-500 focus:ring-4 focus:ring-blue-100"
        />
      </section>

      <section className="grid gap-6 xl:grid-cols-[minmax(360px,0.85fr)_minmax(620px,1.15fr)]">
        <div className="space-y-4">
          <div className="border-2 border-slate-900 bg-white p-4 shadow-[5px_5px_0_#111827]">
            <div className="grid gap-3 sm:grid-cols-2">
              <label className="space-y-1 text-xs font-black uppercase tracking-[0.12em] text-slate-500">
                Khóa
                <select
                  value={courseFilter}
                  onChange={(event) => setCourseFilter(event.target.value)}
                  className="form-control rounded-lg py-2 text-sm normal-case tracking-normal"
                >
                  {COURSE_OPTIONS.map((option) => (
                    <option key={option.value} value={option.value}>{option.label}</option>
                  ))}
                </select>
              </label>

              <label className="space-y-1 text-xs font-black uppercase tracking-[0.12em] text-slate-500">
                Chủ đề
                <select
                  value={topicFilter}
                  onChange={(event) => setTopicFilter(event.target.value)}
                  className="form-control rounded-lg py-2 text-sm normal-case tracking-normal"
                >
                  <option value="">Tất cả</option>
                  {TOPIC_OPTIONS.map((option) => (
                    <option key={option.value} value={option.value}>{option.label}</option>
                  ))}
                </select>
              </label>
            </div>

            <label className="mt-3 flex items-center gap-2 text-sm font-extrabold text-slate-700">
              <input
                type="checkbox"
                checked={includeInactive}
                onChange={(event) => setIncludeInactive(event.target.checked)}
                className="h-4 w-4 accent-blue-600"
              />
              Hiện câu đã ẩn
            </label>

            <div className="mt-3 flex items-center gap-2 border-2 border-slate-200 bg-slate-50 px-3 py-2">
              <Search size={18} className="text-slate-500" />
              <input
                value={query}
                onChange={(event) => setQuery(event.target.value)}
                placeholder="Tìm theo nội dung..."
                className="w-full bg-transparent text-sm font-bold text-slate-800 outline-none placeholder:text-slate-400"
              />
            </div>
          </div>

          <div className="max-h-[calc(100vh-260px)] space-y-3 overflow-y-auto pr-1">
            {isLoading ? (
              <div className="flex h-40 items-center justify-center border-2 border-slate-900 bg-white shadow-[5px_5px_0_#111827]">
                <Loader2 className="animate-spin text-blue-600" size={28} />
              </div>
            ) : filteredQuestions.length === 0 ? (
              <div className="border-2 border-slate-900 bg-white p-5 text-sm font-black text-slate-500 shadow-[5px_5px_0_#111827]">
                Không có câu hỏi.
              </div>
            ) : filteredQuestions.map((question) => (
              <button
                key={question.id}
                type="button"
                onClick={() => setForm(toQuestionForm(question))}
                className={`w-full border-2 p-4 text-left transition-all ${
                  form.id === question.id
                    ? 'border-slate-900 bg-[#DBEAFE] shadow-[5px_5px_0_#111827]'
                    : 'border-slate-200 bg-white hover:border-slate-900 hover:shadow-[4px_4px_0_#111827]'
                }`}
              >
                <div className="flex items-start justify-between gap-3">
                  <div className="min-w-0">
                    <div className="flex flex-wrap items-center gap-2">
                      <span className="border-2 border-slate-900 bg-white px-2 py-0.5 text-xs font-black uppercase text-slate-900">
                        {question.courseCode}
                      </span>
                      <span className="rounded-full bg-slate-100 px-2 py-1 text-xs font-black text-slate-600">
                        {TOPIC_OPTIONS.find((item) => item.value === question.topic)?.label ?? question.topic}
                      </span>
                      {!question.isActive ? (
                        <span className="rounded-full bg-red-100 px-2 py-1 text-xs font-black text-red-600">Đã ẩn</span>
                      ) : null}
                    </div>
                    <p className="mt-3 line-clamp-2 text-base font-black leading-6 text-slate-900">
                      {question.questionText}
                    </p>
                  </div>
                  <FileQuestion className="mt-1 flex-shrink-0 text-blue-600" size={22} />
                </div>
              </button>
            ))}
          </div>
        </div>

        <form
          className="space-y-5 border-2 border-slate-900 bg-white p-5 shadow-[7px_7px_0_#111827]"
          onSubmit={(event) => {
            event.preventDefault();
            void saveQuestion();
          }}
        >
          <div className="flex flex-col gap-3 border-b-2 border-slate-100 pb-4 lg:flex-row lg:items-center lg:justify-between">
            <div>
              <p className="text-xs font-black uppercase tracking-[0.16em] text-slate-500">
                {form.id ? 'Chỉnh sửa' : 'Tạo mới'}
              </p>
              <h2 className="text-2xl font-black text-slate-900">
                {selectedQuestion ? `#${selectedQuestion.orderIndex} ${selectedQuestion.courseCode.toUpperCase()}` : 'Câu hỏi luyện thi'}
              </h2>
            </div>
            <div className="flex flex-wrap gap-2">
              <button
                type="button"
                onClick={resetForm}
                className="inline-flex h-10 items-center gap-2 border-2 border-slate-900 bg-white px-4 text-sm font-black text-slate-900 shadow-[3px_3px_0_#111827]"
              >
                <RotateCcw size={16} />
                Reset
              </button>
              {form.id ? (
                <button
                  type="button"
                  onClick={deleteQuestion}
                  disabled={isSaving}
                  className="inline-flex h-10 items-center gap-2 border-2 border-slate-900 bg-red-50 px-4 text-sm font-black text-red-600 shadow-[3px_3px_0_#111827] disabled:opacity-60"
                >
                  <Trash2 size={16} />
                  Ẩn
                </button>
              ) : null}
              <button
                type="submit"
                disabled={isSaving}
                className="inline-flex h-10 items-center gap-2 border-2 border-slate-900 bg-[#16A34A] px-4 text-sm font-black text-white shadow-[3px_3px_0_#111827] disabled:opacity-60"
              >
                {isSaving ? <Loader2 size={16} className="animate-spin" /> : <Save size={16} />}
                Lưu
              </button>
            </div>
          </div>

          {message ? (
            <div className="flex items-center gap-2 border-2 border-green-600 bg-green-50 px-4 py-3 text-sm font-black text-green-700">
              <CheckCircle2 size={18} />
              {message}
            </div>
          ) : null}

          {error ? (
            <div className="border-2 border-red-600 bg-red-50 px-4 py-3 text-sm font-black text-red-700">
              {error}
            </div>
          ) : null}

          <div className="grid gap-4 md:grid-cols-4">
            <label className="space-y-1 text-xs font-black uppercase tracking-[0.12em] text-slate-500">
              Khóa
              <select value={form.courseCode} onChange={(event) => updateForm('courseCode', event.target.value)} className="form-control rounded-lg py-2 text-sm normal-case tracking-normal">
                {COURSE_OPTIONS.map((option) => <option key={option.value} value={option.value}>{option.label}</option>)}
              </select>
            </label>
            <label className="space-y-1 text-xs font-black uppercase tracking-[0.12em] text-slate-500">
              Loại
              <select value={form.questionType} onChange={(event) => updateForm('questionType', event.target.value)} className="form-control rounded-lg py-2 text-sm normal-case tracking-normal">
                {TYPE_OPTIONS.map((option) => <option key={option.value} value={option.value}>{option.label}</option>)}
              </select>
            </label>
            <label className="space-y-1 text-xs font-black uppercase tracking-[0.12em] text-slate-500">
              Chủ đề
              <select value={form.topic} onChange={(event) => updateForm('topic', event.target.value)} className="form-control rounded-lg py-2 text-sm normal-case tracking-normal">
                {TOPIC_OPTIONS.map((option) => <option key={option.value} value={option.value}>{option.label}</option>)}
              </select>
            </label>
            <label className="space-y-1 text-xs font-black uppercase tracking-[0.12em] text-slate-500">
              Level
              <select value={form.level} onChange={(event) => updateForm('level', event.target.value)} className="form-control rounded-lg py-2 text-sm normal-case tracking-normal">
                {LEVEL_OPTIONS.map((level) => <option key={level} value={level}>{level}</option>)}
              </select>
            </label>
          </div>

          <div className="grid gap-4 md:grid-cols-[1fr_160px]">
            <label className="space-y-1 text-xs font-black uppercase tracking-[0.12em] text-slate-500">
              Nội dung câu hỏi
              <textarea
                value={form.questionText}
                onChange={(event) => updateForm('questionText', event.target.value)}
                rows={3}
                className="form-control min-h-28 rounded-lg text-base normal-case tracking-normal"
                placeholder="例: 学生 nghĩa là gì?"
              />
            </label>
            <div className="space-y-3">
              <label className="space-y-1 text-xs font-black uppercase tracking-[0.12em] text-slate-500">
                Thứ tự
                <input
                  value={form.orderIndex}
                  onChange={(event) => updateForm('orderIndex', event.target.value)}
                  type="number"
                  className="form-control rounded-lg py-2 text-sm normal-case tracking-normal"
                />
              </label>
              <label className="flex items-center gap-2 pt-6 text-sm font-black text-slate-700">
                <input
                  type="checkbox"
                  checked={form.isActive}
                  onChange={(event) => updateForm('isActive', event.target.checked)}
                  className="h-4 w-4 accent-blue-600"
                />
                Đang hiển thị
              </label>
            </div>
          </div>

          {form.questionType === 'passage' ? (
            <div className="space-y-3 border-2 border-blue-200 bg-blue-50 p-4">
              <label className="space-y-1 text-xs font-black uppercase tracking-[0.12em] text-blue-700">
                Tiêu đề đoạn đọc
                <input
                  value={form.passageTitle}
                  onChange={(event) => updateForm('passageTitle', event.target.value)}
                  className="form-control rounded-lg py-2 text-sm normal-case tracking-normal"
                />
              </label>
              <label className="space-y-1 text-xs font-black uppercase tracking-[0.12em] text-blue-700">
                Nội dung đoạn đọc
                <textarea
                  value={form.passageContent}
                  onChange={(event) => updateForm('passageContent', event.target.value)}
                  rows={4}
                  className="form-control min-h-32 rounded-lg text-base normal-case tracking-normal"
                />
              </label>
            </div>
          ) : null}

          <label className="space-y-1 text-xs font-black uppercase tracking-[0.12em] text-slate-500">
            Giải thích
            <textarea
              value={form.explanation}
              onChange={(event) => updateForm('explanation', event.target.value)}
              rows={3}
              className="form-control min-h-28 rounded-lg text-base normal-case tracking-normal"
            />
          </label>

          <div className="space-y-3">
            <p className="text-xs font-black uppercase tracking-[0.12em] text-slate-500">Đáp án</p>
            <div className="grid gap-3 md:grid-cols-2">
              {form.options.map((option, index) => (
                <div key={option.label} className="border-2 border-slate-200 bg-slate-50 p-3">
                  <div className="mb-2 flex items-center justify-between gap-2">
                    <span className="grid h-8 w-8 place-items-center border-2 border-slate-900 bg-white text-sm font-black text-slate-900">
                      {option.label}
                    </span>
                    <label className="flex items-center gap-2 text-sm font-black text-slate-700">
                      <input
                        type="radio"
                        name="correct-option"
                        checked={option.isCorrect}
                        onChange={() => markCorrect(index)}
                        className="h-4 w-4 accent-green-600"
                      />
                      Đúng
                    </label>
                  </div>
                  <textarea
                    value={option.text}
                    onChange={(event) => updateOption(index, { text: event.target.value })}
                    rows={2}
                    className="form-control min-h-20 rounded-lg text-sm normal-case tracking-normal"
                    placeholder={`Đáp án ${option.label}`}
                  />
                </div>
              ))}
            </div>
          </div>
        </form>
      </section>
    </div>
  );
};
