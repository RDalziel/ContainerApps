FROM rstudio/plumber

WORKDIR /app

RUN apt-get install libz-dev && \
    apt-get install dos2unix

RUN R -e "install.packages('remotes', repos = c(CRAN = 'https://cloud.r-project.org'))"
RUN R -e "remotes::install_github('rstudio/renv')"

COPY . .

RUN  find . -type f -print0 | xargs -0 dos2unix

RUN R -e "renv::install('remotes')"
RUN R -e "renv::restore()" #Restore lockfile
RUN R -e "remotes::install_local(dependencies = FALSE)" #Explicitly install this package itself

EXPOSE 8000

ENTRYPOINT []
CMD ["Rscript", "main.R", "0"]
