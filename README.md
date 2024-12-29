## File Shredder
Shred files, where they are not retrievable anymore using Department of Defence standards DoD 5220.22-M.

## How it works:
1- Select a folder/or a file you want to shred.
2- The Application will Shred the file(s) by executing these tasks in the following order:

- Scramble file binary data by generating a new unique buffer 
  - This step is repeated as many as the user has chosen under the Cycle options.
- Set file size to 0 bytes
- Change the file attributes (Date Created) to 1985
- Change the file attributes (Date Last Accessed) to 1985
- Change the file attributes (Date Last Modified) to 1985

Files > 500 MBs will be skipped for performance reasons (files > 400 MBs with 5 Cycles can take up to 15 minutes)

![image](https://github.com/user-attachments/assets/25da54be-adeb-44cd-8a36-bea781d84f91)

Read more here: https://www.blancco.com/resources/blog-dod-5220-22-m-wiping-standard-method/ 
![image](https://github.com/user-attachments/assets/e4b928aa-e412-4573-9a55-2203e72ff97b)
