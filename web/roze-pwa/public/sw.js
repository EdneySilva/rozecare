self.addEventListener('install', () => {
  console.log('RozeCare service worker installing');
});

self.addEventListener('activate', () => {
  console.log('RozeCare service worker activated');
});

self.addEventListener('fetch', () => {
  // Placeholder for caching strategy
});
