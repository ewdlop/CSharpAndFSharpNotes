// import { defineConfig } from 'vite'
// import react from '@vitejs/plugin-react'

// // https://vitejs.dev/config/
// export default defineConfig({
//   plugins: [react()],
//   build: {
//     outDir: 'build',
//     minify: false,
//     sourcemap: true,
//     rollupOptions: {
//       input: 'src/main.tsx',
//       output: {
//         globals: {
//           react: 'React',
//           'react-dom': 'ReactDOM',
//         },
//         manualChunks: {
//           react: ['react'],
//           'react-dom': ['react-dom'],
//         },
//         entryFileNames: '[name].[hash].js',
//         chunkFileNames: '[name].[hash].js',
//         assetFileNames: '[name].[hash].[ext]',
//       },
//       plugins: [react()],
//     },
//     modulePreload: {
//       polyfill: true,
//     }
//   },
// })