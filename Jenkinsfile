pipeline {
    agent any

    environment {
        CI = 'true'
    }

    stages {

        stage('Checkout Code') {
            steps {
                git branch: 'main',
                    url: 'https://github.com/27-02-03/OrangeHRM-Test-Suite.git'
            }
        }

        stage('Restore Dependencies') {
            steps {
                bat 'dotnet restore'
            }
        }

        stage('Build Project') {
            steps {
                bat 'dotnet build --configuration Release'
            }
        }

        stage('Run Tests') {
            steps {
                bat 'dotnet test --logger "trx"'
            }
        }

       stage('Convert TRX to JUnit') {
            steps {
                bat '"C:\\Users\\DomainUser1.HP_PC\\.dotnet\\tools\\trx2junit.exe" **\\*.trx'
            }
        }

        stage('Publish Test Results') {
            steps {
                junit '**/*.xml'
            }
        }
    }   // ✅ THIS closes stages block

    post {
        always {
            archiveArtifacts artifacts: '**/TestResults/**', fingerprint: true
        }
        success {
            echo '✅ Tests Passed'
        }
        failure {
            echo '❌ Tests Failed'
        }
    }
}   // ✅ THIS closes pipeline block
