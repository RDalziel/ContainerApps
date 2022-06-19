library(plumber)
requireNamespace("yaml")

#* @param filePath of the file to load
#* @get /calc
function(filePath=""){
  list(msg = paste0("The filepath is: '", filePath, "'"))
}
