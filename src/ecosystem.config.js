module.exports = {
   apps: [
      {
         name: 'marketplace-stg',
         script: 'npm start',
         env: {
            NODE_ENV: 'production'
         }
      }
   ]
};