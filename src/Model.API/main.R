args <- commandArgs(trailingOnly=TRUE)
devtools::load_all()

library(plumber)
requireNamespace("yaml")

openAPISpec <- yaml::read_yaml("apiSpec.yml")

portNumber <- ifelse(is.na(args[1]),8000, 8000+as.numeric(args[1]))

pr("inst/plumber/plumber.R") %>%
  pr_set_api_spec(openAPISpec) %>%
  pr_run(host="0.0.0.0", port=portNumber)
