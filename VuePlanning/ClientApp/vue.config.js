module.exports = {
  outputDir: "../wwwroot",
  transpileDependencies: ["vuetify"],

  pluginOptions: {
    i18n: {
      locale: "pl",
      fallbackLocale: "pl",
      localeDir: "locales",
      enableInSFC: false
    }
  }
};
