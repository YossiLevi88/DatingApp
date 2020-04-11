export class Message {
    id: number;
    senderId: number;
    senderPhotoUrl: string;
    senderKnownAs: string;
    recipientId: number;
    recipientPhotoUrl: string;
    recipientKnownAs: string;
    content: string;
    isRead: boolean;
    dateRead: Date;
    messageSent: Date;
}
