-- Database CampusMarket
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- MARK: User
CREATE TABLE "Users" (
    "Id" SERIAL PRIMARY KEY,
    "Username" VARCHAR(64) NOT NULL,
    "Password" VARCHAR(255) NOT NULL,
    "Email" VARCHAR(255) NOT NULL,
    "Nickname" VARCHAR(64) NOT NULL,
    "Bio" TEXT,
    "Avatar" VARCHAR(255),
    "IsAdmin" BOOLEAN NOT NULL DEFAULT FALSE,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "LastLoginAt" TIMESTAMPTZ,
    "StudentId" VARCHAR(64),
    "IsVerified" BOOLEAN NOT NULL DEFAULT FALSE,
    "Rating" FLOAT NOT NULL DEFAULT 0,
    "TotalTransactions" INTEGER NOT NULL DEFAULT 0,
    "FollowersCount" INTEGER NOT NULL DEFAULT 0,
    "FollowingCount" INTEGER NOT NULL DEFAULT 0,
    UNIQUE ("Username"),
    UNIQUE ("Email"),
    UNIQUE ("StudentId")
);

-- MARK: Post
CREATE TYPE "PostCategory" AS ENUM ('general', 'for_sale', 'wanted');
CREATE TYPE "PostStatus" AS ENUM ('active', 'sold_out', 'closed', 'deleted');

CREATE TABLE "Post" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INTEGER NOT NULL REFERENCES "Users"("Id") ON DELETE CASCADE,
    "Title" VARCHAR(255) NOT NULL,
    "Description" TEXT NOT NULL,
    "Category" "PostCategory" NOT NULL,
    "Price" DECIMAL(10,2),  -- NULL for general posts
    "Stock" INTEGER,        -- NULL for general and wanted posts
    "Status" "PostStatus" NOT NULL DEFAULT 'active',
    "ViewCount" INTEGER NOT NULL DEFAULT 0,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CHECK (("Category" = 'general' AND "Price" IS NULL AND "Stock" IS NULL) OR 
           ("Category" = 'for_sale' AND "Price" IS NOT NULL) OR
           ("Category" = 'wanted' AND "Price" IS NOT NULL AND "Stock" IS NULL)),
    CHECK ("Price" >= 0),
    CHECK ("Stock" >= 0)
);

CREATE TABLE "PostImage" (
    "Id" SERIAL PRIMARY KEY,
    "PostId" INTEGER NOT NULL REFERENCES "Post"("Id") ON DELETE CASCADE,
    "ImageUrl" VARCHAR(255) NOT NULL,
    "DisplayOrder" INTEGER NOT NULL DEFAULT 0,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE ("PostId", "DisplayOrder")
);

CREATE TABLE "PostComment" (
    "Id" SERIAL PRIMARY KEY,
    "PostId" INTEGER NOT NULL REFERENCES "Post"("Id") ON DELETE CASCADE,
    "UserId" INTEGER NOT NULL REFERENCES "Users"("Id") ON DELETE CASCADE,
    "ParentCommentId" INTEGER REFERENCES "PostComment"("Id") ON DELETE CASCADE,
    "Content" TEXT NOT NULL,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "IsDeleted" BOOLEAN NOT NULL DEFAULT FALSE
);

CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.UpdatedAt = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER update_post_updated_at
    BEFORE UPDATE ON "Post"
    FOR EACH ROW
    EXECUTE FUNCTION update_updated_at_column();

-- Apply trigger to post_comment table
CREATE TRIGGER update_post_comment_updated_at
    BEFORE UPDATE ON "PostComment"
    FOR EACH ROW
    EXECUTE FUNCTION update_updated_at_column();

-- Create index for common queries
CREATE INDEX idx_post_user_id ON "Post"("UserId");
CREATE INDEX idx_post_category ON "Post"("Category");
CREATE INDEX idx_post_status ON "Post"("Status");
CREATE INDEX idx_post_created_at ON "Post"("CreatedAt");
CREATE INDEX idx_post_comment_post_id ON "PostComment"("PostId");
CREATE INDEX idx_post_comment_user_id ON "PostComment"("UserId");
CREATE INDEX idx_post_image_post_id ON "PostImage"("PostId");

-- MARK: Transaction
CREATE TYPE "TransactionStatus" AS ENUM ('pending', 'completed', 'cancelled');

CREATE TABLE "Transaction" (
    "Id" SERIAL PRIMARY KEY,
    "SellerId" INT NOT NULL,
    "BuyerId" INT NOT NULL,
    "PostId" INT,
    "Amount" NUMERIC(10, 2) NOT NULL,
    "Status" "TransactionStatus" NOT NULL DEFAULT 'pending',
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY ("SellerId") REFERENCES "Users"("Id"),
    FOREIGN KEY ("BuyerId") REFERENCES "Users"("Id"),
    FOREIGN KEY ("PostId") REFERENCES "Post"("Id")
);

CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW."UpdatedAt" = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER update_transaction_updated_at
BEFORE UPDATE ON "Transaction"
FOR EACH ROW
EXECUTE FUNCTION update_updated_at_column();
-- MARK: Chat
CREATE TABLE "Conversation" (
    "Id" SERIAL PRIMARY KEY,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE "ConversationParticipants" (
    "ConversationId" INTEGER NOT NULL REFERENCES "Conversation"("Id") ON DELETE CASCADE,
    "UserId" INTEGER NOT NULL REFERENCES "Users"("Id") ON DELETE CASCADE,
    PRIMARY KEY ("ConversationId", "UserId")
);

CREATE TABLE "Message" (
    "Id" SERIAL PRIMARY KEY,
    "ConversationId" INTEGER NOT NULL REFERENCES "Conversation"("Id") ON DELETE CASCADE,
    "SenderId" INTEGER NOT NULL REFERENCES "Users"("Id") ON DELETE CASCADE,
    "Content" TEXT NOT NULL,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_message_conversation_id ON "Message"("ConversationId");
CREATE INDEX idx_message_sender_id ON "Message"("SenderId");
CREATE INDEX idx_conversation_participants_user_id ON "ConversationParticipants"("UserId");

-- MARK: Star
CREATE TABLE "Star" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INTEGER NOT NULL REFERENCES "Users"("Id") ON DELETE CASCADE,
    "PostId" INTEGER NOT NULL REFERENCES "Post"("Id") ON DELETE CASCADE,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE ("UserId", "PostId")
);

-- MARK: Report
CREATE TYPE "ReportType" AS ENUM ('post', 'comment', 'user');

CREATE TABLE "Report" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INTEGER NOT NULL REFERENCES "Users"("Id") ON DELETE CASCADE,
    "TargetId" INTEGER NOT NULL,
    "TargetType" "ReportType" NOT NULL,
    "Reason" TEXT NOT NULL,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- MARK: Notification

CREATE TYPE "NotificationType" AS ENUM ('post_comment', 'post_like', 'post_star', 'post_report', 'comment_reply', 'comment_like', 'comment_report', 'user_report', 'transaction', 'message');

CREATE TABLE "Notification" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INTEGER NOT NULL REFERENCES "Users"("Id") ON DELETE CASCADE,
    "Type" "NotificationType" NOT NULL,
    "TargetId" INTEGER NOT NULL,
    "IsRead" BOOLEAN NOT NULL DEFAULT FALSE,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- MARK: Pinned 

CREATE TABLE "UserPinnedPost" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INTEGER NOT NULL REFERENCES "Users"("Id") ON DELETE CASCADE,
    "PostId" INTEGER NOT NULL REFERENCES "Post"("Id") ON DELETE CASCADE,
    "PinOrder" INTEGER NOT NULL,
    "PinnedAt" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE ("UserId", "PostId"),          
    UNIQUE ("UserId", "PinOrder"),        
    CHECK ("PinOrder" >= 0),             
    CHECK ("PinOrder" < 5)
);

CREATE INDEX idx_user_pinned_post_user_id ON "UserPinnedPost"("UserId");

CREATE OR REPLACE FUNCTION manage_pin_order()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW."PinOrder" IS NULL THEN
        SELECT COALESCE(MAX("PinOrder") + 1, 0)
        INTO NEW."PinOrder"
        FROM "UserPinnedPost"
        WHERE "UserId" = NEW."UserId";
    END IF;
    
    UPDATE "UserPinnedPost"
    SET "PinOrder" = "PinOrder" + 1
    WHERE "UserId" = NEW."UserId"
    AND "PinOrder" >= NEW."PinOrder"
    AND "Id" != COALESCE(NEW."Id", -1);
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_manage_pin_order
    BEFORE INSERT OR UPDATE ON "UserPinnedPost"
    FOR EACH ROW
    EXECUTE FUNCTION manage_pin_order();

-- MARK: Follow

-- Create a follow relationship table
CREATE TABLE "UserFollow" (
    "Id" SERIAL PRIMARY KEY,
    "FollowerId" INTEGER NOT NULL REFERENCES "Users"("Id") ON DELETE CASCADE,
    "FollowingId" INTEGER NOT NULL REFERENCES "Users"("Id") ON DELETE CASCADE,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    -- Prevent self-following and duplicate follows
    CONSTRAINT prevent_self_follow CHECK ("FollowerId" != "FollowingId"),
    UNIQUE ("FollowerId", "FollowingId")
);

-- Create indices for efficient querying
CREATE INDEX idx_user_follow_follower ON "UserFollow"("FollowerId");
CREATE INDEX idx_user_follow_following ON "UserFollow"("FollowingId");

-- Create a trigger to maintain follower/following counts
CREATE OR REPLACE FUNCTION update_follow_counts()
RETURNS TRIGGER AS $$
BEGIN
    IF TG_OP = 'INSERT' THEN
        -- Increment counts
        UPDATE "Users" SET "FollowersCount" = "FollowersCount" + 1 
        WHERE "Id" = NEW."FollowingId";
        
        UPDATE "Users" SET "FollowingCount" = "FollowingCount" + 1 
        WHERE "Id" = NEW."FollowerId";
    ELSIF TG_OP = 'DELETE' THEN
        -- Decrement counts
        UPDATE "Users" SET "FollowersCount" = "FollowersCount" - 1 
        WHERE "Id" = OLD."FollowingId";
        
        UPDATE "Users" SET "FollowingCount" = "FollowingCount" - 1 
        WHERE "Id" = OLD."FollowerId";
    END IF;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_update_follow_counts
AFTER INSERT
OR DELETE ON "UserFollow" FOR EACH ROW
EXECUTE FUNCTION update_follow_counts();

-- MARK: Tag

CREATE TABLE "Tag" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(50) NOT NULL,
    "Description" TEXT,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UseCount" INTEGER NOT NULL DEFAULT 0
);

CREATE TABLE "PostTag" (
    "Id" SERIAL PRIMARY KEY,
    "PostId" INTEGER NOT NULL REFERENCES "Post"("Id") ON DELETE CASCADE,
    "TagId" INTEGER NOT NULL REFERENCES "Tag"("Id") ON DELETE CASCADE,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE ("PostId", "TagId")
);

CREATE INDEX idx_tag_use_count ON "Tag"("UseCount" DESC);
CREATE INDEX idx_post_tag_post_id ON "PostTag"("PostId");
CREATE INDEX idx_post_tag_tag_id ON "PostTag"("TagId");

CREATE OR REPLACE FUNCTION update_tag_use_count()
RETURNS TRIGGER AS $$
BEGIN
    IF TG_OP = 'INSERT' THEN
        UPDATE "Tag" SET "UseCount" = "UseCount" + 1 
        WHERE "Id" = NEW."TagId";
    ELSIF TG_OP = 'DELETE' THEN
        UPDATE "Tag" SET "UseCount" = "UseCount" - 1 
        WHERE "Id" = OLD."TagId";
    END IF;
    RETURN NULL;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_update_tag_use_count
AFTER INSERT OR DELETE ON "PostTag"
FOR EACH ROW
EXECUTE FUNCTION update_tag_use_count();
