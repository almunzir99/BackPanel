export interface DirectoryModel {
    title: string
    size: number
    parentDirectory: string
    files: File[]
    directories: DirectoryModel[]
    createdAt: string
    lastUpdate: string
  }

  