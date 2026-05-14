type AppLogoProps = {
  className?: string;
  imageClassName?: string;
  alt?: string;
};

export const AppLogo = ({
  className = '',
  imageClassName = '',
  alt = 'JPLearn',
}: AppLogoProps) => {
  return (
    <span className={`block shrink-0 overflow-hidden bg-slate-950 ${className}`}>
      <img
        src="/logo.png"
        alt={alt}
        className={`h-full w-full object-cover object-center ${imageClassName}`}
        draggable={false}
      />
    </span>
  );
};
