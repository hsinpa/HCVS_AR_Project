
export default class EventQueue {

    private queue : Array<any>;
    private queueLength : number;
    private headIndex:number;

    /**
        *Creates an instance of EventQueue.
        * @param {number} expect_queue_size
        */
    constructor(expect_queue_size : number) {
        /** @private */
        this.queue = new Array(expect_queue_size);
        /** @private */
        this.queueLength = 0;
        this.headIndex = 0;
    }
   
    /**
        *Queue Msg
        *
        * @param {Object} infoQueue
        */
    Enqueue(infoQueue : any) {
        this.queue.push(infoQueue);
        this.queueLength ++;
    }
   
    /**
     *
     *
     * @returns {Object}
     */
    Dequeue() {
        if (this.headIndex < this.queueLength) {
            
            let preserveMsg = this.queue[this.headIndex];
            this.headIndex++;

            return preserveMsg;
        } else {
            return null;
        }
    }

    Count() {
       return this.queueLength;
    }

    Clear() {
       this.queue.length = 0;
       this.queueLength = 0;
       this.headIndex = 0;
    }
}
