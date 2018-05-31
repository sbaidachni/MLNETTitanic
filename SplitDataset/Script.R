set.seed(123)

all_data <- read.csv2("train.csv", header = TRUE, sep = ",")

smp_size <- floor(0.8 * nrow(all_data))

train_ind <- sample(seq_len(nrow(all_data)), size = smp_size)

train_data <- all_data[train_ind,]
test_data <- all_data[-train_ind,]
write.csv(test_data, "test_data.csv", row.names = FALSE)
write.csv(train_data, "train_data.csv", row.names = FALSE)


