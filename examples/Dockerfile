FROM node:10

RUN apt-get update && \
  apt-get install -y curl \
  unzip \
  bsdmainutils

# https://docs.microsoft.com/en-us/cli/azure/install-azure-cli-apt?view=azure-cli-latest#install
RUN curl -sL https://aka.ms/InstallAzureCLIDeb | bash

# https://www.terraform.io/downloads.html
RUN curl https://releases.hashicorp.com/terraform/0.12.24/terraform_0.12.24_linux_amd64.zip --output terraform.zip

# unzip terraform
RUN unzip terraform.zip

# move to usr/local/bin directory
RUN mv terraform usr/local/bin

# clean up
RUN rm terraform.zip

WORKDIR /samples

# terraform-no-auth
COPY terraform-no-auth/ terraform-no-auth/

# bash-auth
COPY shared bash-auth
RUN chmod +x bash-auth/configure-auth.sh

# terraform-auth
COPY terraform-auth/ terraform-auth/
COPY shared terraform-auth
RUN chmod +x terraform-auth/configure-auth.sh

CMD ["/bin/bash"]