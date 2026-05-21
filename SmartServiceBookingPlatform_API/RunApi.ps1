Load Data and Display First 5 Rows

import pandas as pd
import numpy as np

df = pd.read_csv('KaggleV2-May-2016.csv')

# 1. Display the first five records [cite: 9]
display(df.head())

Dataset Dimensions

# Get the shape of the dataset
rows, columns = df.shape
print(f"The dataset contains {rows} rows and {columns} columns.")

. Data Information and Types

# Display summary information
df.info()

# Identifying missing values per column
print("\nMissing values in each column:")
print(df.isnull().sum())

Descriptive Statistics

# Calculate descriptive statistics
print(df.describe())

# Interpretation (Markdown comment):
# The 'Age' column shows the average age of patients.
# We need to check for anomalies like negative values or extremely high ages.

Unique Values in Categorical Columns

# Checking unique values for categorical columns
print("Unique values in Gender:", df['Gender'].unique())
print("Unique values in No-show:", df['No-show'].unique())

# Searching for illogical values (e.g., ages < 0)
illogical_ages = df[df['Age'] < 0]
print(f"Rows with negative age: {len(illogical_ages)}")

Duplicate Rows


# Check for duplicates
duplicates = df.duplicated().sum()
print(f"Number of duplicate rows: {duplicates}")

# Remove duplicates if any exist
if duplicates > 0:
    df.drop_duplicates(inplace=True)
    print("Duplicates removed.")

Part 2: Data Cleaning

1 & 2. Handle Illogical and Missing Values

# 1. Handle illogical Age (removing negative values)
df = df[df['Age'] >= 0]

# 2. Handle missing values
# Fill Gender with Mode
gender_mode = df['Gender'].mode()[0]
df['Gender'] = df['Gender'].fillna(gender_mode)

# Fill Age with Mean
age_mean = df['Age'].mean()
df['Age'] = df['Age'].fillna(age_mean)

print("Missing values handled.")

Drop Unnecessary Columns

# Dropping columns like PatientId and AppointmentID as they are unique identifiers
df.drop(['PatientId', 'AppointmentID'], axis=1, inplace=True)
print("Unnecessary columns removed.")

Convert Date Columns to Datetime

# Convert ScheduledDay and AppointmentDay to datetime
df['ScheduledDay'] = pd.to_datetime(df['ScheduledDay'])
df['AppointmentDay'] = pd.to_datetime(df['AppointmentDay'])

# Verify the changes
print(df.dtypes[['ScheduledDay', 'AppointmentDay']])

1. توثيق المسار الإجرائي للمشروع (Implementation Methodology)

مرحلة الاستقصاء والتقييم (Data Exploration): بدأت العملية باستيراد مكتبة Pandas لتحميل بيانات مواعيد المرضى التي تتجاوز 100 ألف سجل. تم إجراء فحص هيكلي شامل للتعرف على أنماط البيانات (Data Types) ، حيث ركزنا على اكتشاف الثغرات الإحصائية والقيم غير المنطقية، مثل رصد أعمار سالبة في عمود Age، لضمان جودة المخرجات النهائية.
+3


هندسة وتنقية البيانات (Data Cleaning): انتقلنا إلى مرحلة المعالجة عبر تصحيح التناقضات المنطقية ؛ حيث تم تعويض القيم المفقودة في عمود الجنس (Gender) باستخدام "المنوال" ، وفي عمود العمر باستخدام "المتوسط الحسابي". كما تم تجريد البيانات من المتغيرات غير المؤثرة (كالأسماء أو الأرقام التعريفية) لتركيز القوة الحوسبية على العوامل ذات الثقل التحليلي.
+3


المعالجة الزمنية (Temporal Conversion): تم تحويل سلاسل البيانات النصية للمواعيد إلى صيغة datetime. هذه الخطوة جوهرية لتحويل التاريخ من مجرد نص إلى متغير زمني يمكن قياسه لفهم الفوارق الزمنية بين حجز الموعد وحضور المريض.



2. الرؤية الاستراتيجية والفوائد العملية (Strategic Insights)

تعظيم الكفاءة التشغيلية: يتيح هذا التحليل للمنشأة الطبية التنبؤ بظاهرة الـ "No-show" قبل وقوعها. من خلال فهم هذه الأنماط، يمكن للإدارة تطبيق نظام "الحجز المزدوج الذكي" أو إعادة توزيع الموارد لتقليل وقت الفراغ الناتج عن غياب المرضى.

صناعة سياسات وقائية مخصصة: بدلاً من التعامل مع المرضى ككتلة واحدة، يساعد المشروع في تقسيم المرضى إلى فئات بناءً على السلوك (العمر، الحالة الصحية، الاستجابة للرسائل). هذا يسمح بتصميم حملات تذكيرية مخصصة للفئات الأكثر غياباً، مما يرفع من معدلات الامتثال للمواعيد.


التحول نحو الإدارة القائمة على البيانات: يوفر المشروع قاعدة معرفية صلبة لصناع القرار في المؤسسات الصحية، حيث يتم استبدال التقديرات الشخصية بإحصائيات دقيقة مستخرجة من حوسبة مفتوحة المصدر ، مما يضمن دقة التخطيط المستقبلي للخدمات الطبية.