type NoticeBlockProps = {
  title: string;
  message: string;
};

export default function NoticeBlock({ title, message }: NoticeBlockProps) {
  return (
    <section className="notice-block" aria-live="polite">
      <strong className="notice-title">{title}</strong>
      <p className="notice-text">{message}</p>
    </section>
  );
}
