pipeline{
    agent any
    stages{
        stage("Cleaning"){
            steps{
                echo "======== cleaning========"
                cleanWs()
            }
        }
        stage("LoadingSource"){
            steps{
                echo "====++++Cloning from GitHub++++===="
                git branch: 'master', url: 'https://github.com/metallage/epamtest.git' 
            }
            post{
                
                success{
                    echo "========Successfully cloned========"
                }
                failure{
                    echo "========Cloning failed========"
                }
            }
        }
        stage("Preparing Cake")        {
            steps{
                echo "====++++Creating build.ps1++++===="
                powershell (returnStdout: false, script: '.\\powershell\\bootstrap_cake.ps1')
            }
        }        
        stage("Build"){
            steps{
                echo "========executing Build========"
                powershell(returnStdout: false, script:  '.\\cake\\build.ps1 -Script ".\\cake\\build.cake"')
            }
            post{
                
                success{
                    echo "========Build executed successfully========"
                }
                failure{
                    echo "========Build execution failed========"
                }
            }
        }
        stage("Publish test reports"){
            steps{
                echo "Serching in 'Cake\\TestResult.xml'"
                nunit testResultsPattern: 'Cake\\TestResult.xml'
            }
        }
    }
    post{
        
        success{
            echo "========pipeline executed successfully ========"
            echo "======== cleaning========"
            cleanWs()
        }
        failure{
            echo "========pipeline execution failed========"
        }
    }
    
}
