import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import './index.css'


type customWindow = {
  reactRender : () => void
} & Window & typeof globalThis

declare const window: customWindow;
// Blazor Window

window.reactRender = () => {
  const root2: HTMLElement | null = document.getElementById('root');
  if(root2) {
    const root = ReactDOM.createRoot(root2 as HTMLElement);
    root.render(
        <App />
    );
  }
}