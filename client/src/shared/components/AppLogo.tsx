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
    <span className={`flex items-center justify-center bg-white overflow-hidden ${className}`}>
      <img
        src="/logo.png"
        alt={alt}
        className={`h-full w-full object-contain ${imageClassName}`}
        draggable={false}
      />
    </span>
  );
};
