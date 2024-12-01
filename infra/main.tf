provider "aws" {
  region = "us-east-1"
}

terraform {
  backend "s3" {
    bucket = "terraform-tfstate-grupo12-fiap-2024-sqs-pagamento"
    key    = "lambda_sqs_pagamento/terraform.tfstate"
    region = "us-east-1"
  }
}

resource "aws_iam_role" "lambda_execution_role" {
  name = "lambda_sqs_pagamento_execution_role"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = "sts:AssumeRole"
        Effect = "Allow"
        Principal = {
          Service = "lambda.amazonaws.com"
        }
      },
    ]
  })
}

resource "aws_iam_policy" "lambda_policy" {
  name        = "lambda_sqs_pagamento_policy"
  description = "IAM policy for Lambda execution"
  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Effect = "Allow"
        Action = [
          "logs:CreateLogGroup",
          "logs:CreateLogStream",
          "logs:PutLogEvents",
          "dynamodb:DeleteItem",
          "dynamodb:GetItem",
          "dynamodb:PutItem",
          "dynamodb:Query",
          "dynamodb:Scan",
          "dynamodb:UpdateItem",
          "dynamodb:DescribeTable",
          "sqs:*"
        ]
        Resource = "*"
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "lambda_execution_policy" {
  role       = aws_iam_role.lambda_execution_role.name
  policy_arn = aws_iam_policy.lambda_policy.arn
}

resource "aws_lambda_function" "pedido_function" {
  function_name = "lambda_sqs_pagamento"
  role          = aws_iam_role.lambda_execution_role.arn
  runtime       = "dotnet8"
  memory_size   = 512
  timeout       = 30
  handler       = "FIAP.TechChallenge.FIAP.TechChallenge.Lambda.SQS.Pagamento::FIAP.TechChallenge.FIAP.TechChallenge.Lambda.SQS.Pagamento.Function::FunctionHandler"
  # Código armazenado no S3
  s3_bucket = "code-lambdas-functions-sqs-pagamento"
  s3_key    = "lambda_sqs_pagamento.zip"
}