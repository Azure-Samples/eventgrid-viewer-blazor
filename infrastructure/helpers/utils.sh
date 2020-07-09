#######################################################
#- function used to print messages
#######################################################
function print() {
    echo "$1...."
}

###############################################################
#- function used to create a random phrase
###############################################################
function generateRandomPhrase() {
    local phrase=$(openssl rand -hex 12)

    echo $phrase
}