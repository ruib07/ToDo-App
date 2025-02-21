import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import tailwindcss from "@tailwindcss/vite";
import { env } from 'process';

export default defineConfig({
    plugins: [plugin(), tailwindcss()],
    server: {
        port: parseInt(env.DEV_SERVER_PORT || '3000'),
    }
});
